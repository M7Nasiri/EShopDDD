using _01.Domain.ValueObjects;
using EShop.Shared.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.CustomerAgg.Repository
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        Task<Customer?> GetWithAddressesTrackingAsync(Guid customerId, CancellationToken cancellationToken = default);

        Task<Customer?> GetWithAddressesNoTrackingAsync(Guid customerId, CancellationToken cancellationToken = default);
    }
}
