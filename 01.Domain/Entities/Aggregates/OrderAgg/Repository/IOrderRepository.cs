using _01.Domain.ValueObjects;
using EShop.Shared.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.OrderAgg.Repository
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<Order?> GetWithItemsTrackingAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task<Order?> GetDraftOrderByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);

        Task<Order?> GetByIdReadOnlyAsync(Guid orderId, CancellationToken cancellationToken = default);
    }
}
