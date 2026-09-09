using EShop.Shared.Query.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CustomerAgg.DTOs
{
    public class CustomerFilterParams : BaseFilterParam
    {
        public string? Search { get; set; }       
        public string? Province { get; set; }      
        public string? City { get; set; }          
    }
}
