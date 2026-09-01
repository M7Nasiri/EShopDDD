using _01.Domain.ValueObjects;
using EShop.Shared.Domain;

namespace _01.Domain.DoamainEvents.Orders
{
    public sealed class OrderFinalizedDomainEvent(
    Guid orderId,
    Guid customerId,
    Money totalPrice) : BaseDomainEvent
    {
        public Guid OrderId { get; private set; } = orderId;
        public Guid CustomerId { get; private set; } = customerId;
        public Money TotalPrice { get; private set; } = totalPrice;
    }
}
