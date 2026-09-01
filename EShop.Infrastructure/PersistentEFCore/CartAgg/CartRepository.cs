using _01.Domain.Entities.Aggregates.CartAgg;
using _01.Domain.Entities.Aggregates.CartAgg.Repository;
using _01.Domain.ValueObjects;
using EShop.Infrastructure._Utilities;
using Microsoft.EntityFrameworkCore;

namespace EShop.Infrastructure.PersistentEFCore.CartAgg
{
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        private readonly ShopContext _dbContext;
        public CartRepository(
            ShopContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Cart?> GetByGuestIdAsync(
            string guestId,
            CancellationToken cancellationToken = default)
        {
            return _dbContext.Set<Cart>()
                .Include(x => x.Items)
                .SingleOrDefaultAsync(
                    x => x.GuestId == guestId,
                    cancellationToken);
        }

        public Task<Cart?> GetByCustomerIdAsync(_01.Domain.Guid customerId, CancellationToken cancellationToken)
        {
            return _dbContext.Set<Cart>()
                 .Include(x => x.Items)
                 .SingleOrDefaultAsync(
                     x => x.CustomerId == customerId,
                     cancellationToken);
        }

    }
}
