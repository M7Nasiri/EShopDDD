using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.ProductAgg.DTOs
{
    public record ProductStockDto(Guid ProductId, int Stock, bool HasStock);
}
