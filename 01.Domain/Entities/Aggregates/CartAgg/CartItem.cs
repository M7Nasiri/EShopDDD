using _01.Domain.ValueObjects;
using EShop.Shared.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.CartAgg
{
    public sealed class CartItem : Entity
    {
        public Id Id { get; private set; }

        public Id ProductId { get; private set; }

        public Quantity Quantity { get; private set; }

        private CartItem()
        {
        }

        internal CartItem(
            Id productId,
            Quantity quantity)
        {
            ArgumentNullException.ThrowIfNull(productId);
            ArgumentNullException.ThrowIfNull(quantity);

            Id = Id.New();
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
