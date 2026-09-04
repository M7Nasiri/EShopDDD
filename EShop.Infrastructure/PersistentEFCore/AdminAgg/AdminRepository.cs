using _01.Domain.Entities.Aggregates.AdminAgg;
using _01.Domain.Entities.Aggregates.AdminAgg.Repository;
using _01.Domain.Entities.Aggregates.CartAgg.Repository;
using EShop.Infrastructure._Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.PersistentEFCore.AdminAgg
{
    public class AdminRepository : BaseRepository<Admin>, IAdminRepository
    {
        private readonly ShopContext _dbContext;
        public AdminRepository(
            ShopContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
