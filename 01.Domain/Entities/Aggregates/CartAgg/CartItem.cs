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
        public static CartItem Create(
            Guid cartId,
            Guid productId,
            Quantity quantity)
        {
            if (cartId == Guid.Empty)
                throw new ArgumentException(nameof(cartId));

            if (productId == Guid.Empty)
                throw new ArgumentException(nameof(productId));

            if (quantity.Value <= 0)
                throw new InvalidOperationException(
                    "Quantity must be greater than zero.");

            return new CartItem(
                productId,
                quantity);
        }


        internal void Increase(
            Quantity quantity)
        {
            ArgumentNullException.ThrowIfNull(quantity);
            Quantity = new Quantity(
                Quantity.Value + quantity.Value);
        }

        public void ChangeQuantity(Quantity newQuantity)
        {
            ArgumentNullException.ThrowIfNull(newQuantity);
            Quantity = newQuantity;
        }
    }
}
