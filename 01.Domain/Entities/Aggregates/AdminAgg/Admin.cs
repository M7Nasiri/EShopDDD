using _01.Domain.ValueObjects;
using EShop.Shared.Domain;
using _01.Domain.ValueObjects;

namespace _01.Domain.Entities.Aggregates.AdminAgg
{
    public class Admin : AggregateRoot
    {
        public Name FullName { get; private set; }


        private Admin()
        {

        }
        public Admin(Name fullName)
        {
            Id = Guid.NewGuid();;
            FullName = fullName;
        }

    }
}
