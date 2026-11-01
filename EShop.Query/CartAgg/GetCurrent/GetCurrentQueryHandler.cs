using _01.Domain.Entities.Aggregates.CartAgg;
using _01.Domain.Entities.Aggregates.CartAgg.Repository;
using _01.Domain.Entities.Aggregates.CustomerAgg;
using Dapper;
using EShop.Infrastructure.Persistent.Dapper;
using EShop.Query.CartAgg.DTOs;
using EShop.Shared.Application.Interfaces.Authentication;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CartAgg.GetCurrent
{
    public class GetCurrentQueryHandler : IQueryHandler<GetCurrentQuery, CartDto?>
    {
        private readonly DapperContext _dapperContext;
        private readonly ICurrentUser _currentUser;
        private readonly IGuestSession _guestSession;

        public GetCurrentQueryHandler(
            DapperContext dapperContext,
            ICurrentUser currentUser,
            IGuestSession guestSession)
        {
            _dapperContext = dapperContext;
            _currentUser = currentUser;
            _guestSession = guestSession;
        }

        public async Task<CartDto?> Handle(GetCurrentQuery request, CancellationToken cancellationToken)
        {
            string whereClause;
            object parameters;

            // تشخیص امن هویت جاری از روی Token یا Session
            if (_currentUser.IsValid() && _currentUser.UserId.HasValue)
            {
                whereClause = "c.CustomerId = @OwnerId AND c.Status = @ActiveStatus";
                parameters = new { OwnerId = _currentUser.UserId.Value, ActiveStatus = 1 };
            }
            else
            {
                var guestId = _guestSession.GetGuestId();
                if (string.IsNullOrWhiteSpace(guestId))
                    return null; // هنوز هیچ سبدی برای این مهمان شکل نگرفته است

                whereClause = "c.GuestId = @OwnerId AND c.Status = @ActiveStatus";
                parameters = new { OwnerId = guestId, ActiveStatus = 1 };
            }

            var sql = $@"
                SELECT 
                    c.Id,
                    c.CreationDate,
                    c.CustomerId,
                    c.GuestId,
                    LTRIM(RTRIM(cust.FullName)) AS CustomerFullName,
                    
                    ci.Id,
                    ci.CreationDate,
                    ci.ProductId,
                    ci.Quantity,
                    ISNULL(p.Name, N'محصول نامشخص') AS ProductTitle,
                    ISNULL(p.UnitPrice, 0) AS UnitPrice,
                    p.ImageName AS ProductImageName
                FROM Carts c
                LEFT JOIN Customers cust ON c.CustomerId = cust.Id
                LEFT JOIN CartItems ci ON c.Id = ci.CartId
                LEFT JOIN Products p ON ci.ProductId = p.Id
                WHERE {whereClause}";

            using var connection = _dapperContext.CreateConnection();
            var cartDictionary = new Dictionary<Guid, CartDto>();

            await connection.QueryAsync<CartDto, CartItemDto, CartDto>(
                new CommandDefinition(sql, parameters, cancellationToken: cancellationToken),
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
                splitOn: "Id"
            );

            return cartDictionary.Values.FirstOrDefault();
        }
    }
}
