using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CommentAgg.DTOs.Admin
{
    public class WhoApprovedCommentDto : BaseDto
    {
        public Guid ProductId { get; init; }
        public Guid CustomerId { get; init; }
        public Guid? AdminId { get; init; }
        public string? AdminFullName { get; init; }
        public string? ProductName { get; init; }
        public string? CustomerFullName { get; init; }
        public string Text { get; init; }

    }
}
