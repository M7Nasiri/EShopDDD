using Dapper;
using EShop.Infrastructure.Persistent.Dapper;
using EShop.Infrastructure.PersistentEFCore;
using EShop.Query.CartAgg.DTOs;
using EShop.Shared.Query;
using Microsoft.EntityFrameworkCore;

namespace EShop.Query.CartAgg.GetCartByCustomerId;

public class GetCartByCustomerIdQueryHandler(DapperContext dapperContext)
    : IQueryHandler<GetCartByCustomerIdQuery, CartDto?>
{
    public async Task<CartDto?> Handle(GetCartByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
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
            Left JOIN Customers cust ON c.CustomerId = cust.Id
            Left JOIN CartItems ci ON c.Id = ci.CartId
            Left JOIN Products p ON ci.ProductId = p.Id
            WHERE c.CustomerId = @CustomerId  AND c.Status = @ActiveStatus";

        using var connection = dapperContext.CreateConnection();

        var cartDictionary = new Dictionary<Guid, CartDto>();

        var result = await connection.QueryAsync<CartDto, CartItemDto, CartDto>(
            sql,
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
            param: new { CustomerId = request.CustomerId ,ActiveStatus = 1 },
            splitOn: "Id" // تقسیم‌بندی بین ستون‌های CartDto و CartItemDto بر اساس شناسه دوم (ci.Id)
        );

        
        return cartDictionary.Values.FirstOrDefault();
    }
}