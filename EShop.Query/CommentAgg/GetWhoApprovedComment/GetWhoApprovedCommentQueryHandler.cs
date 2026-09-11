using Dapper;
using EShop.Infrastructure.Persistent.Dapper;
using EShop.Query.CommentAgg.DTOs.Admin;
using EShop.Query.CommentAgg.DTOs.Product;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CommentAgg.GetWhoApprovedComment
{
    internal class GetWhoApprovedCommentQueryHandler(DapperContext context)
        : IQueryHandler<GetWhoApprovedCommentQuery,WhoApprovedCommentDto?>
    {
        public async Task<WhoApprovedCommentDto?> Handle(GetWhoApprovedCommentQuery request, CancellationToken cancellationToken)
        {
            const string sql = @"
            SELECT 
                pc.Id,
                pc.CreationDate,
                pc.CustomerId,
                pc.ModeratedByUserId As AdminId,
                pc.ProductId,
                pc.Text,

                cu.FullName As CustomerFullName,

                a.FullName As AdminFullName,

                p.Name As ProductName
                
            FROM ProductComments pc
            Left JOIN Customers cu ON pc.CustomerId = cu.Id
            Left JOIN Admins a ON pc.ModeratedByUserId = a.Id
            Inner JOIN Products p ON pc.ProductId = p.Id
            
            WHERE pc.Id = @CommentId AND pc.Status = @ActiveStatus";

            using var connection = context.CreateConnection();


            return await connection.QueryFirstOrDefaultAsync<WhoApprovedCommentDto>
            (sql, param: new { CommentId = request.CommentId, ActiveStatus = 1 });
        }
    }
}
