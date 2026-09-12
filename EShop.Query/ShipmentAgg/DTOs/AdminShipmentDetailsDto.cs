using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.ShipmentAgg.DTOs
{
    public class AdminShipmentDetailsDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? TrackingCode { get; set; }

        public string? AddressTitle { get; set; } 
        public string ReceiverName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Plaque { get; set; } = string.Empty;
    }
}
