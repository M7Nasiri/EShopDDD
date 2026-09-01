using _01.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Exceptions
{
    public sealed class InsufficientStockException
     : EShopDomainException
    {
        public Guid ProductId { get; }

        public int Available { get; }

        public int Requested { get; }

        public InsufficientStockException(
            Guid productId,
            int available,
            int requested)
            : base(
                $"Insufficient stock. " +
                $"Available: {available}, " +
                $"Requested: {requested}.")
        {
            ProductId = productId;
            Available = available;
            Requested = requested;
        }
    }
}
