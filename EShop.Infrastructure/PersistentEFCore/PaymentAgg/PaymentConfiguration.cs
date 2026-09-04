using _01.Domain.Entities.Aggregates.PaymentAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Infrastructure.PersistentEFCore.PaymentAgg
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p => p.Id);


            builder.Property(p => p.OrderId)

                .IsRequired();

            // نگاشت Value Object مبلغ (Money)
            builder.OwnsOne(p => p.Amount, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("Amount")
                    .HasPrecision(18, 2)
                    .IsRequired();

                //money.Property(m => m.Currency)
                //    .HasColumnName("Currency")
                //    .HasMaxLength(10)
                //    .IsRequired();
            });

            builder.Property(p => p.Method)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Status)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.GatewayTransactionId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired(false);

            builder.Property(p => p.RefundTransactionId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired(false);

            builder.OwnsOne(p => p.CreatedAt, date =>
            {
                date.Property(d => d.Value)
                    .HasColumnName("CreatedAt")
                    .IsRequired();
            });
            builder.Property(p => p.Authority)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.HasIndex(p => p.Authority);

            // ایندکس‌ها برای ردگیری سریع تراکنش‌ها و سفارشات
            builder.HasIndex(p => p.OrderId);

            builder.HasIndex(p => p.GatewayTransactionId)
                .HasFilter("[GatewayTransactionId] IS NOT NULL");
        }
    }
}
