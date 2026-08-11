using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace _01.Domain.Entities
{
    public class Admin
    {
        public Guid Id { get; set; }
        public string FullName { get; private set; }
        public List<ProctComment> Comments { get; private set; }
        public List<Product> Products { get; private set; }
        public List<Coupon> Coupons { get; set; }
    }
}
