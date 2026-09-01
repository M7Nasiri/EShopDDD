using _01.Domain.ValueObjects;
using EShop.Shared.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.CartAgg.Repository
{
    public interface ICartRepository : IBaseRepository<Cart>
    {
        Task<Cart?> GetByCustomerIdAsync(
        Guid customerId,
        CancellationToken cancellationToken);

        Task<Cart?> GetByGuestIdAsync(
            string guestId,
            CancellationToken cancellationToken);
    }
}
