using _01.Domain.Entities.Aggregates.ShipmentAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Infrastructure.PersistentEFCore.ShipmentAgg
{
    internal class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.ToTable("Shipments");

            // ۱. کلید اصلی
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                .ValueGeneratedNever();

            // ۲. ارتباط با سفارش (Strongly Typed Id)
            builder.Property(s => s.OrderId)
                .IsRequired();

            // ۳. وضعیت مرسوله (تبدیل Enum به string برای خوانایی بهتر در دیتابیس)
            builder.Property(s => s.Status)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            // ۴. کد رهگیری پستی
            builder.Property(s => s.TrackingCode)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.OwnsOne(s => s.Address, addressBuilder =>
            {
                addressBuilder.Property(a => a.Title)
                   .HasColumnName("Title")
                   .HasMaxLength(100)
                   .IsRequired(false);

                addressBuilder.Property(a => a.ReceiverName)
                    .HasColumnName("ReceiverName")
                    .HasMaxLength(150)
                    .IsRequired();

                addressBuilder.Property(a => a.PhoneNumber)
                    .HasColumnName("PhoneNumber")
                    .HasMaxLength(20)
                    .IsRequired();


                addressBuilder.Property(a => a.Province)
                    .HasColumnName("Province")
                    .HasMaxLength(100)
                    .IsRequired();

                addressBuilder.Property(a => a.City)
                    .HasColumnName("City")
                    .HasMaxLength(100)
                    .IsRequired();

                addressBuilder.Property(a => a.Street)
                    .HasColumnName("Street")
                    .HasMaxLength(100)
                    .IsRequired();

                addressBuilder.Property(a => a.PostalCode)
                    .HasColumnName("PostalCode")
                    .HasMaxLength(20)
                    .IsRequired();

                addressBuilder.Property(a => a.Plaque)
                    .HasColumnName("Plaque")
                    .HasMaxLength(20)
                    .IsRequired();

            });

            builder.HasIndex(s => s.OrderId).IsUnique();
            builder.HasIndex(s => s.TrackingCode);
            builder.HasIndex(s => s.Status);
        }
    }
}
