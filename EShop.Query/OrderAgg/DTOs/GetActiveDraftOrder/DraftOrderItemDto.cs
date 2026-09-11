using System;
using System.Collections.Generic;
using System.Text;
using EShop.Shared.Application;
using EShop.Shared.Query;

namespace EShop.Query.OrderAgg.DTOs.GetActiveDraftOrder
{
    public record DraftOrderItemDto(
        Guid ProductId,
        int Count,
        decimal UnitPrice,
        decimal TotalPrice);

    
}
