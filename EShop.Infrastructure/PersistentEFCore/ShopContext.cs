using _01.Domain.Entities.Aggregates.AdminAgg;
using _01.Domain.Entities.Aggregates.CartAgg;
using _01.Domain.Entities.Aggregates.CategoryAgg;
using _01.Domain.Entities.Aggregates.CommentAgg;
using _01.Domain.Entities.Aggregates.CouponAgg;
using _01.Domain.Entities.Aggregates.CustomerAgg;
using _01.Domain.Entities.Aggregates.MembershipAgg;
using _01.Domain.Entities.Aggregates.MembershipPlanAgg;
using _01.Domain.Entities.Aggregates.OrderAgg;
using _01.Domain.Entities.Aggregates.PaymentAgg;
using _01.Domain.Entities.Aggregates.ProductAgg;
using _01.Domain.Entities.Aggregates.ShipmentAgg;
using EShop.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EShop.Infrastructure.PersistentEFCore
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {
            
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductComment> comments { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<MembershipPlan> Plans { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Shipment> Shipments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShopContext).Assembly);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                    continue;

                var parameter = Expression.Parameter(
                    entityType.ClrType,
                    "e");

                var isDeleteProperty = Expression.Property(
                    parameter,
                    nameof(BaseEntity.IsDelete));

                var notDeletedExpression = Expression.Equal(
                    isDeleteProperty,
                    Expression.Constant(false));

                var lambda = Expression.Lambda(
                    notDeletedExpression,
                    parameter);

                entityType.SetQueryFilter(lambda);
            }
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            base.OnConfiguring(optionsBuilder);
            
        }
    }
}
