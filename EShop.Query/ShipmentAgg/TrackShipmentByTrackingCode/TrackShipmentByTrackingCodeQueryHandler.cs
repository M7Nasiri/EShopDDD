using EShop.Infrastructure.PersistentEFCore;
using EShop.Query.ShipmentAgg.DTOs;
using EShop.Shared.Query;
using Microsoft.EntityFrameworkCore;

namespace EShop.Query.ShipmentAgg.TrackShipmentByTrackingCode;

public class TrackShipmentByTrackingCodeQueryHandler(ShopContext context)
    : IQueryHandler<TrackShipmentByTrackingCodeQuery, TrackingShipmentResultDto?>
{
    public async Task<TrackingShipmentResultDto?> Handle(TrackShipmentByTrackingCodeQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.TrackingCode))
            return null;

        var normalizedTrackingCode = request.TrackingCode.Trim();

        return await context.Shipments
            .AsNoTracking()
            .Where(s => s.TrackingCode == normalizedTrackingCode)
            .Select(s => new TrackingShipmentResultDto(
                s.Id,
                s.OrderId,
                s.Status.ToString(),
                s.TrackingCode!,
                s.Address.ReceiverName,
                s.Address.City,
                s.Address.Province
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
}