using _01.Domain.ValueObjects;
using EShop.Shared.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.CommentAgg.Repository
{
    public interface ICommentRepository : IBaseRepository<ProductComment>
    {
        Task<bool> ExistsByProductAndCustomerAsync(
       Guid productId,
       Guid customerId,
       CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ProductComment>> GetApprovedByProductIdAsync(
            System.Guid productId,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ProductComment>> GetPendingAsync(
            CancellationToken cancellationToken = default);

        void Remove(ProductComment comment);
    }
}
