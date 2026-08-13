using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace _01.Domain.Entities.Aggregates.AdminAgg
{
    public class Admin : AggregateRoot
    {
        public Id Id { get; private set; }
        public Name FullName { get; private set; }


        private Admin()
        {
            
        }
        public Admin(Id id,Name fullName)
        {
            Id = id;
            FullName = fullName;
        }
        
    }
}
