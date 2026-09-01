using _01.Domain.Entities.Aggregates.PaymentAgg;
using _01.Domain.Entities.Aggregates.PaymentAgg.Repository;
using _01.Domain.ValueObjects;
using EShop.Infrastructure._Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.PersistentEFCore.PaymentAgg
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        private readonly ShopContext _dbContext;

        public PaymentRepository(ShopContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Payment?> GetByGatewayTransactionIdAsync(string transactionId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Payment>()
            .FirstOrDefaultAsync(p => p.GatewayTransactionId == transactionId, cancellationToken);
        }

        public async Task<Payment?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Payment>()
           .Where(p => p.OrderId == orderId)
           .OrderByDescending(p => p.CreatedAt.Value)
           .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
