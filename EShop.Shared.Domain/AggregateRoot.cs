using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EShop.Shared.Domain
{

    public class AggregateRoot : BaseEntity
    {
        private readonly List<BaseDomainEvent> _domainEvents = new List<BaseDomainEvent>();

        [NotMapped]
        public List<BaseDomainEvent> DomainEvents => _domainEvents;

        public void AddDomainEvent(BaseDomainEvent eventItem)
        {
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(BaseDomainEvent eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
