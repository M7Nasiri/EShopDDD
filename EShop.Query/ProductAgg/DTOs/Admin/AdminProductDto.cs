using System;
using System.Collections.Generic;
using System.Text;
using EShop.Shared.Query;

namespace EShop.Query.ProductAgg.DTOs.Admin
{
    public class AdminProductDto : BaseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Stock { get; set; }
        public Guid CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public Guid CreatedByUserId { get; set; }
        public string? AdderName { get; set; }

        public string? ImageName { get; set; }

    }
}
