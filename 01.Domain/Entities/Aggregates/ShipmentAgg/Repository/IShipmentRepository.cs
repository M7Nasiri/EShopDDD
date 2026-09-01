using _01.Domain.ValueObjects;
using EShop.Shared.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.ShipmentAgg.Repository
{
    public interface IShipmentRepository : IBaseRepository<Shipment>
    {
        Task<Shipment?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task<Shipment?> GetByTrackingCodeAsync(string trackingCode, CancellationToken cancellationToken = default);
    }
}
