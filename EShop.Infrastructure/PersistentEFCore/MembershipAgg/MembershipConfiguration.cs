using _01.Domain.Entities.Aggregates.MembershipAgg;
using _01.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.PersistentEFCore.MembershipAgg
{
    public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.ToTable("Memberships");

            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id);

            builder.Property(m => m.CustomerId)
                .IsRequired();

            builder.Property(m => m.PlanId)
                .IsRequired();

            builder.Property(m => m.DiscountPercent)
                .IsRequired();

            builder.Property(m => m.FreeShipping)
                .IsRequired();

            builder.Property(m => m.PlanTitle)
           .HasMaxLength(150)
           .IsRequired();

            // نگاشت DomainDate (Value Object)
            builder.OwnsOne(m => m.StartDate, date =>
            {
                date.Property(d => d.Value)
                    .HasColumnName("StartDate")
                    .IsRequired();
            });

            builder.OwnsOne(m => m.EndDate, date =>
            {
                date.Property(d => d.Value)
                    .HasColumnName("EndDate")
                    .IsRequired();
            });

            builder.Property(m => m.Status)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            // ایندکس جهت بالا رفتن سرعت کوئری‌های بررسی اشتراک کاربر
            builder.HasIndex(m => new { m.CustomerId, m.Status });

            builder.HasIndex(m => new { m.Status, m.EndDate.Value })
           .HasFilter("[Status] = 1"); // 1 = Active
        }
    }
}
