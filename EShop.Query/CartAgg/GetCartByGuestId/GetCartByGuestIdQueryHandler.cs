using Dapper;
using EShop.Infrastructure.Persistent.Dapper;
using EShop.Query.CartAgg.DTOs;
using EShop.Shared.Query;

namespace EShop.Query.CartAgg.GetCartByGuestId;

public class GetCartByGuestIdQueryHandler : IQueryHandler<GetCartByGuestIdQuery, CartDto?>
{
    private readonly DapperContext _dapperContext;

    public GetCartByGuestIdQueryHandler(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<CartDto?> Handle(GetCartByGuestIdQuery request, CancellationToken cancellationToken)
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
            LEFT JOIN CartItems ci ON c.Id = ci.CartId
            LEFT JOIN Products p ON ci.ProductId = p.Id
            WHERE c.GuestId = @GuestId";


        using var connection = _dapperContext.CreateConnection();

        var cartDictionary = new Dictionary<Guid, CartDto>();

        await connection.QueryAsync<CartDto, CartItemDto, CartDto>(
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
            param: new { GuestId = request.GuestId },
            splitOn: "Id"
        );

        return cartDictionary.Values.FirstOrDefault();
    }
}