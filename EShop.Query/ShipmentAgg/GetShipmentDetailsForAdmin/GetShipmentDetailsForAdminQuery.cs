using EShop.Query.ShipmentAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.ShipmentAgg.GetShipmentDetailsForAdmin
{
    public record GetShipmentDetailsForAdminQuery(Guid ShipmentId) : IQuery<AdminShipmentDetailsDto?>;
}
