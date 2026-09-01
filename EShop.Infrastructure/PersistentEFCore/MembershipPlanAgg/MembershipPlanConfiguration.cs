using _01.Domain.Entities.Aggregates.MembershipPlanAgg;
using _01.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.PersistentEFCore.MembershipPlanAgg
{
    public class MembershipPlanConfiguration : IEntityTypeConfiguration<MembershipPlan>
    {
        public void Configure(EntityTypeBuilder<MembershipPlan> builder)
        {
            builder.ToTable("MembershipPlans");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasConversion(id => id.Value, value => new Guid(value));

            // نگاشت Name (Value Object)
            builder.OwnsOne(p => p.Name, name =>
            {
                name.Property(n => n.Value)
                    .HasColumnName("Name")
                    .HasMaxLength(150)
                    .IsRequired();
                name.HasIndex(n => n.Value).IsUnique();
            });

            // نگاشت Money (Value Object)
            builder.OwnsOne(p => p.Price, price =>
            {
                price.Property(m => m.Amount)
                    .HasColumnName("Price")
                    .HasPrecision(18, 2)
                    .IsRequired();
            });

            // نگاشت MembershipDuration (Value Object)
            builder.OwnsOne(p => p.DurationInDays, duration =>
            {
                duration.Property(d => d.Days)
                    .HasColumnName("DurationInDays")
                    .IsRequired();
            });

            builder.Property(p => p.DiscountPercent)
                .IsRequired();

            builder.Property(p => p.FreeShipping)
                .IsRequired();

            builder.Property(p => p.IsActive)
                .IsRequired();
        }
    }
}
