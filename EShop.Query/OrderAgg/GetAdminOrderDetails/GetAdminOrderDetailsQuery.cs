using EShop.Query.OrderAgg.DTOs.GetAdminOrderDetails;
using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;
using EShop.Shared.Query;

namespace EShop.Query.OrderAgg.GetAdminOrderDetails
{
    public record GetAdminOrderDetailsQuery(Guid OrderId) : IQuery<AdminOrderDetailDto?>;

}
