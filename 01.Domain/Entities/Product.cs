using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public List<Dictionary<string,string>> Attributes { get; private set; }
        public List<Order> Orders { get; private set; }
        public List<Admin> Inserter { get; private set; }
        public decimal UnitPrice { get; set; }
    }
}
