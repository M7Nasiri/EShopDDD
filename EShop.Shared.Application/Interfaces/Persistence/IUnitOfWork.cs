using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Shared.Application.Interfaces.Persistence
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default);
    }
}
