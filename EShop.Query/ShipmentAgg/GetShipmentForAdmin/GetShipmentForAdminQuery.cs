using EShop.Query.ShipmentAgg.DTOs.Admin;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;

namespace EShop.Query.ShipmentAgg.GetShipmentForAdmin
{
    public class GetShipmentsForAdminQuery(ShipmentAdminFilterParams filterParams)
        : QueryFilter<ShipmentAdminFilterResult, ShipmentAdminFilterParams>(filterParams)
    {
    }
}
