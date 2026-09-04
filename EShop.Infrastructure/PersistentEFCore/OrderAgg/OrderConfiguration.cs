using _01.Domain.Entities.Aggregates.OrderAgg;
using _01.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Infrastructure.PersistentEFCore.OrderAgg
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .ValueGeneratedNever();

            builder.Property(o => o.CustomerId)
                .IsRequired();

            builder.Property(o => o.Status)
                .HasConversion<int>()
                .IsRequired();


            builder.Property(o => o.CreatedAt)
               .HasConversion(d => d.Value, v => new DomainDate(v))
               .HasColumnType("datetime2")
               .IsRequired();

            builder.Property(o => o.BaseShippingCost)
                 .HasConversion(m => m.Amount, v => new Money(v))
                 .HasColumnName("BaseShippingCost")
                 .HasPrecision(18, 2)
                 .IsRequired();




            // نگاشت آدرس ارسال (Owned Type)
            builder.OwnsOne(o => o.ShippingAddress, addr =>
            {
                addr.Property(a => a.ReceiverName)
                  .HasColumnName("ShippingReceiverName")
                  .HasMaxLength(150)
                  .IsRequired();

                addr.Property(a => a.PhoneNumber)
                    .HasColumnName("ShippingPhoneNumber")
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
                coupon.Property(c => c.Code).HasColumnName("CouponCode").HasMaxLength(50).IsUnicode(false);
                coupon.Property(c => c.Percent).HasColumnName("CouponPercent");
            });

            // نگاشت اسنپ‌شات تخفیف اشتراک
            builder.OwnsOne(o => o.MembershipDiscount, discount =>
            {
                discount.Property(d => d.Reason).HasColumnName("MembershipDiscountTitle").HasMaxLength(100);
                discount.Property(d => d.Percent).HasColumnName("MembershipDiscountPercent");
                discount.Property(d => d.FreeShipping).HasColumnName("MembershipFreeShipping");

            });

            builder.Navigation(o => o.Items)
               .HasField("_items")
               .UsePropertyAccessMode(PropertyAccessMode.Field);

            // ارتباط یک‌به‌چند با OrderItem با استفاده از فیلد پشتیبان (_items)
            builder.OwnsMany(o => o.Items, item =>
            {
                item.ToTable("OrderItems");
                item.WithOwner().HasForeignKey("OrderId");

                item.HasKey(i => i.Id);

                item.Property(i => i.Id)
                    .ValueGeneratedNever();

                item.Property(i => i.ProductId)
                    .IsRequired();

                // نگاشت Value Object تعداد (Quantity)
                item.Property(i => i.Quantity)
                    .HasConversion(q => q.Value, v => new Quantity(v))
                    .HasColumnName("Quantity")
                    .IsRequired();

                item.Property(i => i.UnitPrice)
                    .HasConversion(m => m.Amount, v => new Money(v))
                    .HasColumnName("UnitPrice")
                    .HasPrecision(18, 2)
                    .IsRequired();
            });


            // ایندکس ترکیبی برای جستجوی سریع سفارش‌های پیش‌نویس کاربر
            builder.HasIndex(o => new { o.CustomerId, o.Status });
        }
    }

}