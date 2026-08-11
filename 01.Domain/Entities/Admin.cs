using _01.Domain.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace _01.Domain.Entities
{
    public class Admin
    {
        public Id Id { get; set; }
        public FullName FullName { get; private set; }
        public List<ProductComment> Comments { get; private set; }
        public List<Product> Products { get; private set; }
        public List<Coupon> Coupons { get; set; }
    }
}
