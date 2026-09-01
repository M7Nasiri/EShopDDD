using _01.Domain.Entities.Aggregates.CartAgg;
using _01.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.PersistentEFCore.CartAgg
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Carts");

            // Primary Key با تبدیل Value Object شناسه
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id);

            // شناسه کاربر (اختیاری برای کاربر ثبت‌نام کرده)
            builder.Property(c => c.CustomerId)
                .HasConversion(
                    id => id != null ? id.Value : (Guid?)null,
                    value => value.HasValue ? new Guid(value.Value) : null)
                .IsRequired(false);

            // شناسه مهمان (اختیاری برای سبد مهمان)
            builder.Property(c => c.GuestId)
                .HasMaxLength(100)
                .IsUnicode(false) // برای رشته‌های کاراکتری استاندارد مانند کوکی یا GUID
                .IsRequired(false);

            // تنظیم دسترسی مستقیم EF Core به فیلد خصوصی کالکشن
            builder.Navigation(c => c.Items)
                .HasField("_items")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            // رابطه یک به چند با CartItem و حذف آبشاری
            builder.OwnsMany(c => c.Items, options =>
            {
                options.ToTable("CartItems");

                options.HasKey(ci => ci.Id);
                options.Property(ci => ci.Id)
                    .HasConversion(id => id.Value, value => new MyId(value));

                // کلید خارجی مرجع کالا
                options.Property(ci => ci.ProductId)
                    .HasConversion(id => id.Value, value => new Guid(value))
                    .IsRequired();

                // نگاشت Value Object تعداد (Quantity)
                options.Property(ci => ci.Quantity)
                    .HasConversion(q => q.Value, value => new Quantity(value))
                    .IsRequired();

                // ایندکس ترکیبی یونیک روی (CartId, ProductId)
                // این ایندکس تضمین می‌کند که یک کالا دو بار در جدول به صورت ردیف مجزا برای یک سبد ثبت نشود
                options.HasIndex("CartId", nameof(CartItem.ProductId))
                    .IsUnique();
            });
                

            // ایندکس‌ها برای تسریع در واکشی سبد خرید
            builder.HasIndex(c => c.CustomerId)
                .HasFilter("[CustomerId] IS NOT NULL");

            builder.HasIndex(c => c.GuestId)
                .HasFilter("[GuestId] IS NOT NULL");
        }
    }
}
