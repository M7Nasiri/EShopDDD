using EShop.Query.OrderAgg.DTOs.GetActiveDraftOrder;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.OrderAgg.GetActiveDraftOrder
{
    public record GetActiveDraftOrderQuery(Guid CustomerId) : IQuery<DraftOrderDto?>;
}
