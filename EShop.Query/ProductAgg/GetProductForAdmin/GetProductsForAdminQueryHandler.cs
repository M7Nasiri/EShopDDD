using Dapper;
using EShop.Infrastructure.Persistent.Dapper;
using EShop.Query.ProductAgg.DTOs.Admin;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.ProductAgg.GetProductForAdmin
{
    internal class GetProductsForAdminQueryHandler(DapperContext dapperContext)
    : IQueryHandler<GetProductsForAdminQuery, ProductAdminFilterResult>
    {
        public async Task<ProductAdminFilterResult> Handle(GetProductsForAdminQuery request, CancellationToken cancellationToken)
        {
            var filter = request.FilterParams;
            var dynamicParams = new DynamicParameters();
            var condition = new StringBuilder("WHERE 1=1 ");

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                condition.Append(" AND p.Name LIKE @Search ");
                dynamicParams.Add("Search", $"%{filter.Search.Trim()}%");
            }

            if (filter.CategoryId.HasValue && filter.CategoryId.Value != Guid.Empty)
            {
                condition.Append(" AND p.CategoryId = @CategoryId ");
                dynamicParams.Add("CategoryId", filter.CategoryId.Value);
            }

            if (filter.OnlyInStock == true)
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
                p.CategoryId,
                ca.Name As CategoryName,
                p.ImageName, 
                p.CreationDate,
                p.CreatedByUserId,
                a.FullName As AdderName
            FROM Products p
            Left Join Categories ca On p.CategoryId = ca.Id
            Left Join Admins a On p.CreatedByUserId = a.Id
            {condition}
            ORDER BY p.CreationDate DESC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";

            var products = (await connection.QueryAsync<AdminProductDto>(
                new CommandDefinition(sql, dynamicParams, cancellationToken: cancellationToken)
            )).ToList();

            var result = new ProductAdminFilterResult();
            result.GeneratePaging(totalCount, filter.Take, filter.PageId);
            result.Data = products;

            return result;
        }
    }
}
