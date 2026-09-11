using System;
using System.Collections.Generic;
using System.Text;
using EShop.Query.CommentAgg.DTOs.Admin;
using EShop.Shared.Query;

namespace EShop.Query.CommentAgg.GetWhoApprovedComment
{
    public sealed record GetWhoApprovedCommentQuery(Guid CommentId) : IQuery<WhoApprovedCommentDto?>;
}
