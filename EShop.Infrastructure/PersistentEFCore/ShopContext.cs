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
using _01.Domain.ValueObjects;
using EShop.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
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
                // الف) نباید Owned باشد (حل خطای فعلی)
                if (entityType.IsOwned())
                    continue;

                // ب) باید از نوع AggregateRoot (یا اگر ترجیح می‌دهید BaseEntity غیر Owned) باشد
                if (!typeof(AggregateRoot).IsAssignableFrom(entityType.ClrType))
                    continue;

                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var isDeleteProperty = Expression.Property(parameter, nameof(BaseEntity.IsDelete));
                var notDeletedExpression = Expression.Equal(isDeleteProperty, Expression.Constant(false));

                var lambda = Expression.Lambda(notDeletedExpression, parameter);
                entityType.SetQueryFilter(lambda);
            }


            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            base.OnConfiguring(optionsBuilder);
            
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder.Properties<Money>()
                .HaveConversion<MoneyConverter>();

            configurationBuilder.Properties<Quantity>()
                .HaveConversion<QuantityConverter>();

            configurationBuilder.Properties<Name>()
                .HaveConversion<NameConverter>();

            configurationBuilder.Properties<DomainDate>()
                .HaveConversion<DomainDateConverter>();

            configurationBuilder.Properties<Weight>()
                .HaveConversion<WeightConverter>();
        }

        public class MoneyConverter : ValueConverter<Money, decimal>
        {
            public MoneyConverter() : base(m => m.Amount, v => new Money(v)) { }
        }
        public class QuantityConverter : ValueConverter<Quantity, int>
        {
            public QuantityConverter() : base(m => m.Value, v => new Quantity(v)) { }
        }
        public class NameConverter : ValueConverter<Name, string>
        {
            public NameConverter() : base(m => m.Value, v => new Name(v)) { }
        }
        public class DomainDateConverter : ValueConverter<DomainDate, DateTime>
        {
            public DomainDateConverter() : base(m => m.Value, v => new DomainDate(v)) { }
        }
        public class WeightConverter : ValueConverter<Weight, double>
        {
            public WeightConverter() : base(m => m.Kg, v => new Weight(v)) { }
        }
    }
}
