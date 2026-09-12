using EShop.Query.ShipmentAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.ShipmentAgg.TrackShipmentByTrackingCode
{
    public record TrackShipmentByTrackingCodeQuery(string TrackingCode) : IQuery<TrackingShipmentResultDto?>;
}
