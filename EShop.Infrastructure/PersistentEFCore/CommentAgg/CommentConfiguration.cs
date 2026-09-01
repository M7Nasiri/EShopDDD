using _01.Domain.Entities.Aggregates.CommentAgg;
using _01.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.PersistentEFCore.CommentAgg
{
    public class CommentConfiguration : IEntityTypeConfiguration<ProductComment>
    {
        public void Configure(EntityTypeBuilder<ProductComment> builder)
        {
            builder.ToTable("ProductComments");

            builder.HasKey(pc => pc.Id);
            builder.Property(pc => pc.Id);

            builder.Property(pc => pc.ProductId)
                .IsRequired();

            builder.Property(pc => pc.CustomerId)
                .IsRequired();

            builder.Property(pc => pc.ModeratedByUserId)
                .IsRequired(false);

            // متن کامنت با طول استاندارد و پشتیبانی از یونیکد (فارسی)
            builder.Property(pc => pc.Text)
                .HasMaxLength(1500)
                .IsRequired();

            builder.Property(pc => pc.Status)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            // ایندکس برای لود کامنت‌های تایید شده در صفحه جزییات محصول
            // سرعت واکشی کامنت‌های هر کالا را بسیار بالا می‌برد
            builder.HasIndex(pc => new { pc.ProductId, pc.Status });

            // ایندکس برای بخش مدیریت و پنل ادمین (فیلتر کردن کامنت‌های معلق)
            builder.HasIndex(pc => pc.Status);
        }
    }
}
