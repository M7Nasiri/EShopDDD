using _01.Domain.Entities.Aggregates.CustomerAgg;
using _01.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Infrastructure.PersistentEFCore.CustomerAgg
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // ۱. نام جدول اصلی
            builder.ToTable("Customers");


            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedNever();


            builder.Property(c => c.FullName)
                .HasConversion(n => n.Value, v => new Name(v))
                .HasColumnName("FullName")
                .HasMaxLength(200)
                .IsRequired();

            builder.Navigation(c => c.Addresses)
               .HasField("_addresses")
               .UsePropertyAccessMode(PropertyAccessMode.Field);


            // ۴. نگاشت لیست آدرس‌ها (Owned Collection)
            builder.OwnsMany(c => c.Addresses, a =>
            {
                a.ToTable("CustomerAddresses");              // جدول مجزا برای آدرس‌ها
                a.WithOwner().HasForeignKey("CustomerId");   // کلید خارجی به مشتری

                a.Property<int>("Id").ValueGeneratedOnAdd();                       // کلید اصلی مخفی (Shadow PK)
                a.HasKey("Id");

                a.Property(a => a.ReceiverName)
                    .HasColumnName("ReceiverName")
                    .HasMaxLength(150)
                    .IsRequired();

                a.Property(a => a.PhoneNumber)
                    .HasColumnName("PhoneNumber")
                    .HasMaxLength(20)
                    .IsRequired();
                a.Property(x => x.Title).HasMaxLength(100).IsRequired();
                a.Property(x => x.Province).HasMaxLength(100).IsRequired();
                a.Property(x => x.City).HasMaxLength(100).IsRequired();
                a.Property(x => x.Street).HasMaxLength(300).IsRequired();
                a.Property(x => x.Plaque).HasMaxLength(50).IsRequired();
                a.Property(x => x.PostalCode).HasMaxLength(10).IsFixedLength().IsRequired();
            });

            // ۵. نگاشت آدرس پیش‌فرض (Owned Entity تکی)
            builder.OwnsOne(c => c.DefaultAddress, da =>
            {
                da.Property(x => x.ReceiverName)
             .HasColumnName("Default_ReceiverName")
             .HasMaxLength(150)
             .IsRequired(false);

                da.Property(x => x.PhoneNumber)
                    .HasColumnName("Default_PhoneNumber")
                    .HasMaxLength(20)
                    .IsRequired(false);
                da.Property(x => x.Title).HasColumnName("Default_Title").HasMaxLength(100);
                da.Property(x => x.Province).HasColumnName("Default_Province").HasMaxLength(100);
                da.Property(x => x.City).HasColumnName("Default_City").HasMaxLength(100);
                da.Property(x => x.Street).HasColumnName("Default_Street").HasMaxLength(300);
                da.Property(x => x.Plaque).HasColumnName("Default_Plaque").HasMaxLength(50);
                da.Property(x => x.PostalCode).HasColumnName("Default_PostalCode").HasMaxLength(10);
            });

        }
    }
}
