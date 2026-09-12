using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.ShipmentAgg.DTOs
{
    public class OrderShipmentDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? TrackingCode { get; set; }
        public string ReceiverName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string FormattedAddress { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
    }
}
