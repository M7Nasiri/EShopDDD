using _01.Domain.Consts;
using EShop.Shared.Query.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CommentAgg.DTOs.Admin
{
    public class CommentFilterParams : BaseFilterParam
    {
        public string? Search { get; set; }            
        public ProductCommentStatus? Status { get; set; }
        public Guid? ProductId { get; set; }  
        public Guid? CustomerId { get; set; }  
        
    }
}
