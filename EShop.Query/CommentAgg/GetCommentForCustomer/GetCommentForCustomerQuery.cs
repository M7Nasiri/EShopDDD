using EShop.Query.CommentAgg.DTOs.Admin;
using EShop.Query.CommentAgg.DTOs.Customer;
using EShop.Shared.Query;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CommentAgg.GetCommentForCustomer
{
    public class GetCommentForCustomerQuery(Guid customerId, CommentCustomerFilterParams filterParams)
        : QueryFilter<CommentCustomerFilterResult, CommentCustomerFilterParams>(filterParams)
    {
        public Guid CustomerId { get; } = customerId;
    }

}
