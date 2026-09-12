using System.Text;
using Dapper;
using EShop.Infrastructure.Persistent.Dapper;
using EShop.Query.ProductAgg.DTOs.Customer;
using EShop.Shared.Query;

namespace EShop.Query.ProductAgg.GetProductsShopFilter;

internal class GetProductsShopFilterQueryHandler(DapperContext dapperContext)
    : IQueryHandler<GetProductsShopFilterQuery, ShopProductFilterResult>
{
    public async Task<ShopProductFilterResult> Handle(GetProductsShopFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.FilterParams;
        var dynamicParams = new DynamicParameters();
        var condition = new StringBuilder("WHERE 1=1 ");

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            condition.Append(" AND (p.Name LIKE @Search OR p.Description LIKE @Search) ");
            dynamicParams.Add("Search", $"%{filter.Search.Trim()}%");
        }

        if (filter.CategoryId.HasValue && filter.CategoryId.Value != Guid.Empty)
        {
            condition.Append(" AND p.CategoryId = @CategoryId ");
            dynamicParams.Add("CategoryId", filter.CategoryId.Value);
        }

        if (filter.OnlyAvailable)
        {
            condition.Append(" AND p.Stock > 0 ");
        }

        if (filter.MinPrice.HasValue)
        {
            condition.Append(" AND p.UnitPrice >= @MinPrice ");
            dynamicParams.Add("MinPrice", filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            condition.Append(" AND p.UnitPrice <= @MaxPrice ");
            dynamicParams.Add("MaxPrice", filter.MaxPrice.Value);
        }

        var orderBy = filter.SortBy switch
        {
            ProductSortBy.Cheapest => "p.UnitPrice ASC",
            ProductSortBy.Expensive => "p.UnitPrice DESC",
            _ => "p.CreationDate DESC"
        };

        using var connection = dapperContext.CreateConnection();

        var countSql = $"SELECT COUNT(1) FROM Products p {condition}";
        var totalCount = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(countSql, dynamicParams, cancellationToken: cancellationToken)
        );

        var skip = (filter.PageId - 1) * filter.Take;
        dynamicParams.Add("Skip", skip);
        dynamicParams.Add("Take", filter.Take);

        var sql = $@"
            SELECT 
                p.Id, 
                p.Name, 
                p.UnitPrice, 
                p.Stock, 
                p.ImageName AS MainImage,
                p.CategoryId
            FROM Products p
            {condition}
            ORDER BY {orderBy}
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";

        var items = (await connection.QueryAsync<ShopProductItemDto>(
            new CommandDefinition(sql, dynamicParams, cancellationToken: cancellationToken)
        )).ToList();

        var result = new ShopProductFilterResult();
        result.GeneratePaging(totalCount, filter.Take, filter.PageId);
        result.Data = items;

        return result;
    }
}