using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace _01.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string FullName { get; private set; }
        public List<Ordere> Orders { get; private set; }
        public List<ProdcutComment> Comments { get; private set; }
        public List<Address> Address { get; private set; }
    }
}
