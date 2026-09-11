using _01.Domain.ValueObjects;
using EShop.Shared.Domain;
using _01.Domain.ValueObjects;

namespace _01.Domain.Entities.Aggregates.AdminAgg
{
    public class Admin : AggregateRoot
    {
        public Name FullName { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; } // یا string
        public Email Email { get; private set; }

        private Admin()
        {

        }
        public Admin(Guid id,Name fullName, PhoneNumber phoneNumber, Email email)
        {
            Id = id;
            FullName = fullName;
            PhoneNumber = phoneNumber;
            Email = email;
        }

    }
}
