using _01.Domain.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities
{
    public class OrderItem
    {
        public Id Id { get; set; }
        //public Guid OrderId { get; private set; }
        public Guid ProductId { get;private set; }
        public Count Count { get;private set; }
        public Money UnitPrice { get; private set; }
        public Money TotalPrice => UnitPrice * Count.Value;
        public OrderItem()
        {
            
        }
        public OrderItem(Guid productId,int count,Money unitPrice)
        {
            ProductId = productId;
            Count = count;
            UnitPrice = unitPrice;
        }
    }
}
