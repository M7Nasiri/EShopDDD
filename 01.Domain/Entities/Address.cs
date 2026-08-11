using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Name { get;private set; }
        public string Value { get; private set; }
        public Guid CustomerId { get; private set; }
    }
}
