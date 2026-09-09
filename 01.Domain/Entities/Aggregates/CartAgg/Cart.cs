using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using _01.Domain.Consts;

namespace _01.Domain.Entities.Aggregates.CartAgg
{
    public sealed class Cart : AggregateRoot
    {
        private readonly List<CartItem> _items = new();


        public Guid? CustomerId { get; private set; }

        public string? GuestId { get; private set; }

        public IReadOnlyCollection<CartItem> Items =>
            _items.AsReadOnly();

        public CartStatus Status { get; private set; }

        private Cart()
        {
        }

        public Cart(
            Guid? customerId)
        {
            if (customerId == Guid.Empty)
                throw new EShopDomainException("Customer id is invalid.");

            Id = Guid.NewGuid();;
            CustomerId = customerId;
        }


        public Cart(
            string guestId)
        {
            if (string.IsNullOrWhiteSpace(guestId))
                throw new EShopDomainException(
                    "Guest id is required.");

            Id = Guid.NewGuid();;
            GuestId = guestId.Trim();
        }

        public static Cart CreateForCustomer(Guid? customerId)
        {
            return new Cart(customerId);
        }
        public static Cart CreateForGuest(string guestId)
        {
            return new Cart(guestId);
        }


        public void AddItem(
            Guid productId,
            Quantity quantity,
            Quantity availableStock)
        {
            ArgumentNullException.ThrowIfNull(quantity);
            ArgumentNullException.ThrowIfNull(availableStock);

            if (productId == Guid.Empty)
                throw new EShopDomainException("ProductId cannot be empty.");

            var item = _items.FirstOrDefault(
                x => x.ProductId == productId);

            int finalQuantity = 0;
            if (item is null)
            {
                finalQuantity = Math.Min(quantity.Value, availableStock.Value);

                _items.Add(
                    new CartItem(
                        productId: productId,
                        quantity: new Quantity(finalQuantity)));

                return;
            }


            var currentQuantity = _items
                .FirstOrDefault(x => x.ProductId == productId)?
                .Quantity.Value ?? 0;

            finalQuantity = currentQuantity + quantity.Value;

            if (finalQuantity > availableStock.Value)
            {
                throw new EShopDomainException(
                    "Requested quantity exceeds available stock.");
            }

            item.Increase(quantity);

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

        public void AssignToCustomer(Guid customerId)
        {
            if (customerId == Guid.Empty)
                throw new EShopDomainException(
                    "CustomerId cannot be empty.");

            if (CustomerId.HasValue && CustomerId != customerId)
                throw new InvalidOperationException(
                    "This cart belongs to another customer.");

            CustomerId = customerId;
            GuestId = null;
        }

        //public void MergeItem(
        //    Guid productId,
        //    int quantity,
        //    int availableStock)
        //{
        //    if (quantity <= 0 || availableStock <= 0)
        //        return;

        //    var existingItem = _items
        //        .FirstOrDefault(x => x.ProductId == productId);

        //    if (existingItem is null)
        //    {
        //        _items.Add(
        //            new CartItem(
        //                productId: productId,
        //                new Quantity(Math.Min(quantity, availableStock))));

        //        return;
        //    }

        //    var mergedQuantity =
        //        Math.Min(
        //            existingItem.Quantity.Value + quantity,
        //            availableStock);

        //    existingItem.ChangeQuantity(new Quantity(mergedQuantity));
        //}
    }
}
