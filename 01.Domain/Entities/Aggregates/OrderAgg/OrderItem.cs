using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Domain;

namespace _01.Domain.Entities.Aggregates.OrderAgg
{
    public sealed class OrderItem : BaseEntity
    {

        public Guid ProductId { get; private set; }

        public Quantity Quantity { get; private set; }

        public Money UnitPrice { get; private set; }

        public Money TotalPrice =>
            UnitPrice * Quantity.Value;

        private OrderItem()
        {
        }

        internal OrderItem(
            Guid productId,
            Quantity quantity,
            Money unitPrice)
        {
            ArgumentNullException.ThrowIfNull(productId);
            ArgumentNullException.ThrowIfNull(quantity);
            ArgumentNullException.ThrowIfNull(unitPrice);

            if (quantity.Value < 1)
                throw new EShopDomainException(
                    "Order item quantity must be at least one.");

            Id = Guid.New();
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        internal void IncreaseQuantity(
            Quantity quantity)
        {
            ArgumentNullException.ThrowIfNull(quantity);

            Quantity = new Quantity(
                Quantity.Value + quantity.Value);
        }

        internal void DecreaseQuantity(
            Quantity quantity)
        {
            ArgumentNullException.ThrowIfNull(quantity);

            var newValue =
                Quantity.Value - quantity.Value;

            if (newValue < 1)
                throw new EShopDomainException(
                    "Order item quantity cannot be less than one.");

            Quantity = new Quantity(newValue);
        }

        internal void ChangeUnitPrice(
            Money newPrice)
        {
            ArgumentNullException.ThrowIfNull(newPrice);

            UnitPrice = newPrice;
        }
    }
}
