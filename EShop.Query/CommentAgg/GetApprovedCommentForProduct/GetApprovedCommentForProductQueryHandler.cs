using EShop.Infrastructure.Persistent.Dapper;
using EShop.Query.CartAgg.DTOs;
using EShop.Query.CommentAgg.DTOs.Product;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;

namespace EShop.Query.CommentAgg.GetApprovedCommentForProduct
{
    public class GetApprovedCommentForProductQueryHandler(DapperContext context) 
        : IQueryHandler<GetApprovedCommentForProductQuery, IReadOnlyList<ProductCommentSummaryDto?>>
    {
        public async Task<IReadOnlyList<ProductCommentSummaryDto?>> Handle(GetApprovedCommentForProductQuery request,
            CancellationToken cancellationToken)
        {
            const string sql = @"
            SELECT 
                pc.Id,
                pc.CreationDate,
                pc.CustomerId,
                pc.ProductId,
                pc.Text,
                cu.FullName As CustomerFullName
                
            FROM ProductComments pc
            Left JOIN Customers cu ON pc.CustomerId = cu.Id
            Left JOIN Products p ON pc.ProductId = p.Id
            WHERE pc.ProductId = @productId  AND pc.Status = @ActiveStatus";

            using var connection = context.CreateConnection();


            var result = await connection.QueryAsync<ProductCommentSummaryDto>(
                sql, param: new { productId = request.ProductId, ActiveStatus = 1 });
            return result.ToList();
        }
    }
}
