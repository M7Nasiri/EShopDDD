using _01.Domain.ValueObjects;
using EShop.Shared.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.PaymentAgg.Repository
{
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        Task<Payment?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task<Payment?> GetByGatewayTransactionIdAsync(string transactionId, CancellationToken cancellationToken = default);

    }
}
