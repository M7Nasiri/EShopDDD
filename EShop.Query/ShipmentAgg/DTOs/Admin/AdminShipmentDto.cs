using System;
using System.Collections.Generic;
using System.Text;
using EShop.Shared.Query;

namespace EShop.Query.ShipmentAgg.DTOs.Admin
{
    public class AdminShipmentDto : BaseDto
    {
        public Guid OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? TrackingCode { get; set; }
        public string ReceiverName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
    }
}
