using _01.Domain.Entities.Aggregates.ShipmentAgg;
using _01.Domain.Entities.Aggregates.ShipmentAgg.Repository;
using _01.Domain.ValueObjects;
using EShop.Infrastructure._Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.PersistentEFCore.ShipmentAgg
{
    public class ShipmentRepository : BaseRepository<Shipment>, IShipmentRepository
    {
        private readonly ShopContext _context;

        public ShipmentRepository(ShopContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Shipment?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Shipment>()
           .FirstOrDefaultAsync(s => s.OrderId == orderId, cancellationToken);
        }

        public async Task<Shipment?> GetByTrackingCodeAsync(string trackingCode, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Shipment>()
            .FirstOrDefaultAsync(s => s.TrackingCode == trackingCode, cancellationToken);
        }
    }
}
