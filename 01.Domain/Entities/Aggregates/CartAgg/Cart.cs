using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.CartAgg
{
    public sealed class Cart : AggregateRoot
    {
        private readonly List<CartItem> _items = new();

        public Id Id { get; private set; }

        public Id? CustomerId { get; private set; }

        public string? GuestId { get; private set; }

        public IReadOnlyCollection<CartItem> Items =>
            _items.AsReadOnly();

        private Cart()
        {
        }

        public Cart(
            Id id,
            Id customerId)
        {
            Id = id;
            CustomerId = customerId;
        }

        public Cart(
            Id id,
            string guestId)
        {
            if (string.IsNullOrWhiteSpace(guestId))
                throw new EShopDomainException(
                    "Guest id is required.");

            Id = id;
            GuestId = guestId;
        }

        public void AddItem(
            Id productId,
            Quantity quantity)
        {
            ArgumentNullException.ThrowIfNull(productId);
            ArgumentNullException.ThrowIfNull(quantity);

            if (quantity.Value < 1)
                throw new EShopDomainException(
                    "Quantity must be greater than zero.");

            var item =
                _items.FirstOrDefault(
                    x => x.ProductId == productId);

            if (item is not null)
            {
                item.Increase(quantity);
                return;
            }

            _items.Add(
                new CartItem(
                    productId,
                    quantity));
        }

        public void RemoveItem(Id productId)
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
