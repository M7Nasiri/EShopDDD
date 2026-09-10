using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using EShop.Infrastructure.Persistent.Dapper;
using EShop.Query.CommentAgg.DTOs.Admin;
using EShop.Shared.Query;
using EShop.Query.CommentAgg.DTOs.Customer;

namespace EShop.Query.CommentAgg.GetAllCommentForAdmin
{
    internal class GetAllCommentForAdminQueryHandler(DapperContext context)
        : IQueryHandler<GetAllCommentForAdminQuery, CommentFilterResult>
    {


        public async Task<CommentFilterResult> Handle(
            GetAllCommentForAdminQuery request,
            CancellationToken cancellationToken)
        {
            var filter = request.FilterParams;
            var dynamicParams = new DynamicParameters();

            var whereConditions = new StringBuilder("WHERE pc.IsDelete = 0");


            if (filter.Status.HasValue)
            {
                whereConditions.Append(" AND pc.Status = @Status");
                dynamicParams.Add("Status", filter.Status.Value.ToString());
            }

            if (filter.ProductId.HasValue)
            {
                whereConditions.Append(" AND pc.ProductId = @ProductId");
                dynamicParams.Add("ProductId", filter.ProductId.Value);
            }
            if (filter.CustomerId.HasValue)
            {
                whereConditions.Append(" AND pc.CustomerId = @CustomerId");
                dynamicParams.Add("ProductId", filter.CustomerId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                whereConditions.Append(@" AND (
                    pc.Text LIKE @Search 
                    OR c.FullName LIKE @Search 
                    OR p.Title LIKE @Search
                )");
                dynamicParams.Add("Search", $"%{filter.Search.Trim()}%");
            }

 
            var countSql = $@"
                SELECT COUNT(1)
                FROM ProductComments pc
                INNER JOIN Customers c ON pc.CustomerId = c.Id
                INNER JOIN Products p ON pc.ProductId = p.Id
                {whereConditions};";

            using var connection = context.CreateConnection();
            var totalCount = await connection.ExecuteScalarAsync<int>(
                new CommandDefinition(countSql, dynamicParams, cancellationToken: cancellationToken)
            );

            var skip = (filter.PageId - 1) * filter.Take;
            dynamicParams.Add("Skip", skip);
            dynamicParams.Add("Take", filter.Take);

            var querySql = $@"
                SELECT 
                    pc.Id,
                    pc.ProductId,
                    p.Name AS ProductName,
                    p.ImageName AS ProductImage,
                    pc.CustomerId,
                    c.FullName AS CustomerFullName,
                    pc.Text,
                    pc.Status,
                    pc.CreationDate
                FROM ProductComments pc
                INNER JOIN Customers c ON pc.CustomerId = c.Id
                INNER JOIN Products p ON pc.ProductId = p.Id
                {whereConditions}
                ORDER BY pc.CreationDate DESC
                OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";

            var comments = (await connection.QueryAsync<CommentAdminSummaryDto>(
                new CommandDefinition(querySql, dynamicParams, cancellationToken: cancellationToken)
            )).AsList();

            var result = new CommentFilterResult()
            {
                Data = comments
            };
            result.GeneratePaging(totalCount, filter.Take, filter.PageId);
            return result;
        }

    }
}
