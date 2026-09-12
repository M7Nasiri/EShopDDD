using EShop.Infrastructure.PersistentEFCore;
using EShop.Query.ProductAgg.DTOs;
using EShop.Shared.Query;
using Microsoft.EntityFrameworkCore;

namespace EShop.Query.ProductAgg.CheckProductStock;

public class CheckProductStockQueryHandler(ShopContext context)
    : IQueryHandler<CheckProductStockQuery, ProductStockDto?>
{
    public async Task<ProductStockDto?> Handle(CheckProductStockQuery request, CancellationToken cancellationToken)
    {
        return await context.Products
            .AsNoTracking()
            .Where(p => p.Id == request.ProductId)
            .Select(p => new ProductStockDto(
                p.Id,
                p.Stock.Value,
                p.Stock.Value >= request.RequiredCount
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
}