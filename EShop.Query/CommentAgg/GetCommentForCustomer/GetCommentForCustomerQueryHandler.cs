using Dapper;
using EShop.Infrastructure.Persistent.Dapper;
using EShop.Query.CommentAgg.DTOs.Admin;
using EShop.Query.CommentAgg.DTOs.Customer;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;
using _01.Domain.Exceptions;
using EShop.Shared.Application.Interfaces.Authentication;

namespace EShop.Query.CommentAgg.GetCommentForCustomer
{
    public class GetCommentForCustomerQueryHandler(DapperContext context,ICurrentUser currentUser) : IQueryHandler<GetCommentForCustomerQuery,CommentCustomerFilterResult>
    {
        public async Task<CommentCustomerFilterResult> Handle(GetCommentForCustomerQuery request, CancellationToken cancellationToken)
        {
            if (currentUser.UserId != request.CustomerId)
                throw new EShopDomainException("You don't have access!");

            var filter = request.FilterParams;
            var dynamicParams = new DynamicParameters();


            var whereConditions = new StringBuilder("WHERE pc.CustomerId=@CustomerId And pc.IsDelete = 0");
            dynamicParams.Add("CustomerId", request.CustomerId);

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


            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                whereConditions.Append(@" AND (
                    pc.Text LIKE @Search 
                    OR p.Title LIKE @Search
                )");
                dynamicParams.Add("Search", $"%{filter.Search.Trim()}%");
            }

            
            var countSql = $@"
                SELECT COUNT(1)
                FROM ProductComments pc
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

            var comments = (await connection.QueryAsync<CommentCustomerSummaryDto>(
                new CommandDefinition(querySql, dynamicParams, cancellationToken: cancellationToken)
            )).AsList();


            var result = new CommentCustomerFilterResult
            {
                Data = comments
            };

            result.GeneratePaging(totalCount, filter.Take, filter.PageId);
            return result;
        }
    }
}
