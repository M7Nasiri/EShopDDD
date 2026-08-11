using _01.Domain.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities
{
    public class Order
    {
        public Id Id { get; set; }
        public Guid CustomerId { get; private set; }
        public List<OrderItem> Items { get; private set; }
        public Money TotalPrice {
            get
            {
                var total = Items.Sum(x => x.TotalPrice.Amount);
                var coupon = total * (decimal)((Coupon.Value) / 100);
                total = total - coupon;
                return new Money(total);
            }
        }
        public Coupon Coupon { get; private set; }
        public bool IsFinally { get; private set; }
        public bool IsCouponed { get; private set; }

    }
}
