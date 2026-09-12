using EShop.Query.ProductAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.ProductAgg.CheckProductStock
{
    public record CheckProductStockQuery(Guid ProductId, int RequiredCount = 1) : IQuery<ProductStockDto?>;
}
