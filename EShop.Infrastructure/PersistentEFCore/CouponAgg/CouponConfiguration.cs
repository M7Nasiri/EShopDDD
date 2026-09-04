using _01.Domain.Entities.Aggregates.CouponAgg;
using _01.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Infrastructure.PersistentEFCore.CouponAgg
{
    public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            // ۱. نام جدول
            builder.ToTable("Coupons");

            // ۲. کلید اصلی
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            // ۳. کد تخفیف (یکتا، بدون کاراکتر یونیکد، طول ثابت/مشخص)
            builder.Property(c => c.Code)
                .HasMaxLength(50)
                .IsUnicode(false) // کدهای تخفیف معمولاً حروف انگلیسی و اعداد هستند
                .IsRequired();

            // ایندکس یکتا روی کد تخفیف جهت جلوگیری از ثبت کد تکراری و افزایش سرعت جستجو
            builder.HasIndex(c => c.Code)
                .IsUnique();

            // ۴. درصد تخفیف
            builder.Property(c => c.Percent)
                .IsRequired();

            // ۵. شناسه ایجادکننده
            builder.Property(c => c.CreatedByUserId)
                .IsRequired();

            // ۶. تاریخ شروع و پایان (تبدیل Value Object به DateTime در دیتابیس)
            // نکته: اگر پراپرتی زیرین DomainDate نام دیگری مثل Value دارد، d.Value را بگذارید
            builder.Property(c => c.StartDate)
                .HasConversion(
                    d => d.Value,
                    v => new DomainDate(v))
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(c => c.EndDate)
                .HasConversion(
                    d => d.Value,
                    v => new DomainDate(v))
                .HasColumnType("datetime2")
                .IsRequired();

            // ۷. وضعیت فعال/غیرفعال
            builder.Property(c => c.IsActive)
                .IsRequired();

            // ۸. سقف استفاده و تعداد استفاده‌شده
            builder.Property(c => c.UsageLimit)
                .IsRequired(false);

            builder.Property(c => c.UsedCount)
                .IsRequired();

            // ۹. ایندکس ترکیبی برای اعتبارسنجی سریع کوپن‌های فعال در بازه زمانی
            builder.HasIndex(c => new { c.Code, c.IsActive });
        }
    }
}
