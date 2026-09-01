using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.CartAgg
{
    public sealed class Cart : AggregateRoot
    {
        private readonly List<CartItem> _items = new();


        public Guid? CustomerId { get; private set; }

        public string? GuestId { get; private set; }

        public IReadOnlyCollection<CartItem> Items =>
            _items.AsReadOnly();

        private Cart()
        {
        }

        public Cart(
            Guid customerId)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
        }

        public Cart(
            string guestId)
        {
            if (string.IsNullOrWhiteSpace(guestId))
                throw new EShopDomainException(
                    "Guest id is required.");

            Id = Guid.NewGuid();
            GuestId = guestId.Trim();
        }


        public void AddItem(
            Guid productId,
            Quantity quantity,
            Quantity availableStock)
        {
            ArgumentNullException.ThrowIfNull(productId);
            ArgumentNullException.ThrowIfNull(quantity);
            ArgumentNullException.ThrowIfNull(availableStock);

            var currentQuantity = _items
                .FirstOrDefault(x => x.ProductId == productId)?
                .Quantity.Value ?? 0;

            var finalQuantity = currentQuantity + quantity.Value;

            if (finalQuantity > availableStock.Value)
            {
                throw new EShopDomainException(
                    "Requested quantity exceeds available stock.");
            }

            var item = _items.FirstOrDefault(
                x => x.ProductId == productId);

            if (item is not null)
            {
                item.Increase(quantity);
                return;
            }

            _items.Add(new CartItem(productId, quantity));
        }

        public void RemoveItem(Guid productId)
        {
            ArgumentNullException.ThrowIfNull(productId);

            var item =
                _items.FirstOrDefault(
                    x => x.ProductId == productId);

            if (item is null)
                return;

            _items.Remove(item);
        }

        public void Clear()
        {
            _items.Clear();
        }
    }
}
