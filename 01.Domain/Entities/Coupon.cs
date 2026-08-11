using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities
{
    public class Coupon
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public int Percent { get; private set; }
    }
}
