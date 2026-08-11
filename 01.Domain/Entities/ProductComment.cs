using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities
{
    public class ProductComment
    {
        public Guid Id { get; set; }
        public string Text { get; private set; }
        public Guid ProductId { get;private set; }
        public Guid CustomerId { get; private set; }
        public Guid AdminId { get; private set; }
        public bool Status { get; private set; }
       
    }
}
