using Dapper;
using EShop.Infrastructure.Persistent.Dapper;
using EShop.Query.ProductAgg.DTOs.Details;
using EShop.Shared.Query;

namespace EShop.Query.ProductAgg.GetProductDetailsById;

internal class GetProductDetailsByIdQueryHandler(DapperContext dapperContext)
    : IQueryHandler<GetProductDetailsByIdQuery, ProductDetailsDto?>
{
    public async Task<ProductDetailsDto?> Handle(GetProductDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        using var connection = dapperContext.CreateConnection();

  
        const string sql = @"
            SELECT 
                p.Id, 
                p.Name, 
                p.Description, 
                p.UnitPrice, 
                p.Stock, 
                p.CategoryId, 
                p.ImageName AS MainImageو
                ca.Name As CategoryName
            FROM Products p
            Left Join Categories ca
            On p.CategoryId = ca.Id
            WHERE p.Id = @ProductId;

            SELECT Id, ImageName, Sequence 
            FROM ProductImages 
            WHERE ProductId = @ProductId 
            ORDER BY Sequence ASC;

            SELECT [Key], [Value] 
            FROM ProductAttributes 
            WHERE ProductId = @ProductId;";

        using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, new { ProductId = request.ProductId }, cancellationToken: cancellationToken)
        );

        var product = await multi.ReadFirstOrDefaultAsync<ProductDetailsDto>();
        if (product == null)
            return null;

        product.Images = (await multi.ReadAsync<ProductImageDto>()).ToList();
        product.Attributes = (await multi.ReadAsync<ProductAttributeDto>()).ToList();

        return product;
    }
}