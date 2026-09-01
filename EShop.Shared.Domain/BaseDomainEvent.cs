using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Shared.Domain
{
    public class BaseDomainEvent : INotification
    {
        public DateTime CreationDate { get; protected set; }

        public BaseDomainEvent()
        {
            CreationDate = DateTime.Now;
        }
    }
}
