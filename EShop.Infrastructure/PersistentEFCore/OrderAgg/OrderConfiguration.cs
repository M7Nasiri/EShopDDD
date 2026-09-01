using _01.Domain.Entities.Aggregates.OrderAgg;
using _01.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.PersistentEFCore.OrderAgg
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .HasConversion(id => id.Value, value => new MyId(value))
                .ValueGeneratedNever();

            builder.Property(o => o.CustomerId)
                .HasConversion(id => id.Value, value => new Guid(value))
                .IsRequired();

            builder.Property(o => o.Status)
                .HasConversion<int>()
                .IsRequired();

            // نگاشت Value Object تاریخ ایجاد
            builder.OwnsOne(o => o.CreatedAt, date =>
            {
                date.Property(d => d.Value)
                    .HasColumnName("CreatedAt")
                    .IsRequired();
            });

            // نگاشت آدرس ارسال (Owned Type)
            builder.OwnsOne(o => o.ShippingAddress, addr =>
            {
                addr.Property(a => a.ReceiverName)
              .HasColumnName("ReceiverName")
              .HasMaxLength(150)
              .IsRequired();

                addr.Property(a => a.PhoneNumber)
                    .HasColumnName("PhoneNumber")
                    .HasMaxLength(20)
                    .IsRequired();
                addr.Property(a => a.Title).HasColumnName("ShippingTitle").HasMaxLength(100);
                addr.Property(a => a.Province).HasColumnName("ShippingProvince").HasMaxLength(100);
                addr.Property(a => a.City).HasColumnName("ShippingCity").HasMaxLength(100);
                addr.Property(a => a.Street).HasColumnName("ShippingStreet").HasMaxLength(300);
                addr.Property(a => a.Plaque).HasColumnName("ShippingPlaque").HasMaxLength(50);
                addr.Property(a => a.PostalCode).HasColumnName("ShippingPostalCode").HasMaxLength(20);
            });

            // نگاشت اسنپ‌شات کوپن
            builder.OwnsOne(o => o.AppliedCoupon, coupon =>
            {
                coupon.Property(c => c.Code).HasColumnName("CouponCode").HasMaxLength(50);
                coupon.Property(c => c.Percent).HasColumnName("CouponPercent");
            });

            // نگاشت اسنپ‌شات تخفیف اشتراک
            builder.OwnsOne(o => o.MembershipDiscount, discount =>
            {
                discount.Property(d => d.Reason).HasColumnName("MembershipDiscountTitle").HasMaxLength(100);
                discount.Property(d => d.Percent).HasColumnName("MembershipDiscountPercent");
                discount.Property(d => d.FreeShipping).HasColumnName("Membership_FreeShipping");

            });

            // ارتباط یک‌به‌چند با OrderItem با استفاده از فیلد پشتیبان (_items)
            builder.OwnsMany(o => o.Items, options =>
            {
                options.ToTable("OrderItems");

                options.HasKey(i => i.Id);

                options.Property(i => i.Id)
                    .HasConversion(id => id.Value, value => new MyId(value))
                    .ValueGeneratedNever();

                options.Property(i => i.ProductId)
                    .HasConversion(id => id.Value, value => new Guid(value))
                    .IsRequired();

                // نگاشت Value Object تعداد (Quantity)
                options.OwnsOne(i => i.Quantity, q =>
                {
                    q.Property(x => x.Value)
                        .HasColumnName("Quantity")
                        .IsRequired();
                });

                // نگاشت Value Object قیمت (Money)
                options.OwnsOne(i => i.UnitPrice, m =>
                {
                    m.Property(x => x.Amount)
                        .HasColumnName("UnitPrice")
                        .HasPrecision(18, 2)
                        .IsRequired();
                });
            });


            builder.Navigation(o => o.Items)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            // ایندکس ترکیبی برای جستجوی سریع سفارش‌های پیش‌نویس کاربر
            builder.HasIndex(o => new { o.CustomerId, o.Status });
        }
    }

}