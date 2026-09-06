using Dapper;
using EShop.Infrastructure.Persistent.Dapper;
using EShop.Query.CartAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CartAgg.GetCartByFilter
{
    public class GetCartsByFilterQueryHandler : IQueryHandler<GetCartsByFilterQuery, CartFilterResult>
    {
        private readonly DapperContext _dapperContext;

        public GetCartsByFilterQueryHandler(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<CartFilterResult> Handle(GetCartsByFilterQuery request, CancellationToken cancellationToken)
        {
            var @params = request.FilterParams;
            var skip = (@params.PageId - 1) * @params.Take;

            var whereClauses = new List<string>();
            var dynamicParams = new DynamicParameters();

            if (@params.CustomerId.HasValue)
            {
                whereClauses.Add("c.CustomerId = @CustomerId");
                dynamicParams.Add("CustomerId", @params.CustomerId.Value);
            }

            if (!string.IsNullOrWhiteSpace(@params.GuestId))
            {
                whereClauses.Add("c.GuestId = @GuestId");
                dynamicParams.Add("GuestId", @params.GuestId);
            }

            if (@params.HasItemsOnly == true)
            {
                whereClauses.Add("EXISTS (SELECT 1 FROM CartItems WHERE CartId = c.Id)");
            }

            var whereSql = whereClauses.Any() ? "WHERE " + string.Join(" AND ", whereClauses) : "";

            var countSql = $"SELECT COUNT(1) FROM Carts c {whereSql}";

            var dataSql = $@"
            WITH PagedCarts AS (
                SELECT c.Id, c.CreationDate, c.CustomerId, c.GuestId
                FROM Carts c
                {whereSql}
                ORDER BY c.CreationDate DESC
                OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY
            )
            SELECT 
                pc.Id,
                pc.CreationDate,
                pc.CustomerId,
                pc.GuestId,
                LTRIM(RTRIM(cust.FullName)) AS CustomerFullName,
                
                ci.Id,
                ci.CreationDate,
                ci.ProductId,
                ci.Quantity,
                ISNULL(p.Name, N'محصول نامشخص') AS ProductTitle,
                ISNULL(p.UnitPrice, 0) AS UnitPrice,
                p.ImageName AS ProductImageName
            FROM PagedCarts pc
            LEFT JOIN Customers cust ON pc.CustomerId = cust.Id
            LEFT JOIN CartItems ci ON pc.Id = ci.CartId
            LEFT JOIN Products p ON ci.ProductId = p.Id
            ORDER BY pc.CreationDate DESC";

            dynamicParams.Add("Skip", skip);
            dynamicParams.Add("Take", @params.Take);

            using var connection = _dapperContext.CreateConnection();

            var totalCount = await connection.ExecuteScalarAsync<int>(countSql, dynamicParams);

            var cartDictionary = new Dictionary<Guid, CartDto>();

            await connection.QueryAsync<CartDto, CartItemDto, CartDto>(
                dataSql,
                (cart, item) =>
                {
                    if (!cartDictionary.TryGetValue(cart.Id, out var currentCart))
                    {
                        currentCart = cart;
                        currentCart.Items = new List<CartItemDto>();
                        cartDictionary.Add(currentCart.Id, currentCart);
                    }

                    if (item != null && item.ProductId != Guid.Empty)
                    {
                        currentCart.Items.Add(item);
                    }

                    return currentCart;
                },
                param: dynamicParams,
                splitOn: "Id"
            );

            var result = new CartFilterResult
            {
                FilterParams = @params,
                Data = cartDictionary.Values.ToList()
            };

            result.GeneratePaging(totalCount, @params.Take, @params.PageId);

            return result;
        }
    }
}
