using EShop.Infrastructure.PersistentEFCore;
using EShop.Query.ShipmentAgg.DTOs;
using EShop.Shared.Query;
using Microsoft.EntityFrameworkCore;

namespace EShop.Query.ShipmentAgg.GetShipmentByOrderId;

public class GetShipmentByOrderIdQueryHandler(ShopContext context)
    : IQueryHandler<GetShipmentByOrderIdQuery, OrderShipmentDto?>
{
    public async Task<OrderShipmentDto?> Handle(GetShipmentByOrderIdQuery request, CancellationToken cancellationToken)
    {
        return await context.Shipments
            .AsNoTracking()
            .Where(s => s.OrderId == request.OrderId)
            .Select(s => new OrderShipmentDto
            {
                Id = s.Id,
                OrderId = s.OrderId,
                Status = s.Status.ToString(),
                TrackingCode = s.TrackingCode,
                ReceiverName = s.Address.ReceiverName,
                PhoneNumber = s.Address.PhoneNumber,
                PostalCode = s.Address.PostalCode,
                FormattedAddress = $"{s.Address.Province}، {s.Address.City}، {s.Address.Street}، پلاک {s.Address.Plaque}"
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}