using _01.Domain.ValueObjects;
using EShop.Shared.Domain;

namespace _01.Domain.Entities.Aggregates.CartAgg
{
    public sealed class CartItem : BaseEntity
    {

        public Guid ProductId { get; private set; }

        public Quantity Quantity { get; private set; }

        private CartItem()
        {
        }

        public CartItem(
            Guid productId,
            Quantity quantity)
        {
            ArgumentNullException.ThrowIfNull(quantity);

            Id = Guid.NewGuid();;
            ProductId = productId;
            Quantity = quantity;
        }

        internal void Increase(
            Quantity quantity)
        {
            Quantity = new Quantity(
                Quantity.Value + quantity.Value);
        }
    }
}
