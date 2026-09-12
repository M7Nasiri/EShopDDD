using EShop.Query.ShipmentAgg.DTOs;
using EShop.Shared.Query;


namespace EShop.Query.ShipmentAgg.GetShipmentByOrderId
{
    public record GetShipmentByOrderIdQuery(Guid OrderId) : IQuery<OrderShipmentDto?>;
}
