using _01.Domain.ValueObjects;
using EShop.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.DoamainEvents.Products
{
    public sealed class ProductStockLowDomainEvent(
        Guid productId,
        int currentStock) : BaseDomainEvent
    {
        public Guid ProductId { get; private set; } = productId;
        public int CurrentStock { get; private set; } = currentStock;
    }
}
