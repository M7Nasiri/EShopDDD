using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.ShipmentAgg.DTOs
{
    public record TrackingShipmentResultDto(
        Guid ShipmentId,
        Guid OrderId,
        string Status,
        string TrackingCode,
        string ReceiverName,
        string City,
        string Province
    );
}
