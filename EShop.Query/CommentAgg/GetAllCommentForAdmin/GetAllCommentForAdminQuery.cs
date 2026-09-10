using EShop.Query.CommentAgg.DTOs.Customer;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;
using EShop.Query.CommentAgg.DTOs.Admin;

namespace EShop.Query.CommentAgg.GetAllCommentForAdmin
{
    public class GetAllCommentForAdminQuery(CommentFilterParams filterParams)
        : QueryFilter<CommentFilterResult, CommentFilterParams>(filterParams);
}
