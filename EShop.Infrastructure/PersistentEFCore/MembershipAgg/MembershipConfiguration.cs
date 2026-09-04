using _01.Domain.Entities.Aggregates.MembershipAgg;
using _01.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Infrastructure.PersistentEFCore.MembershipAgg
{
    public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
    {

        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.ToTable("Memberships");

            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id)
                .ValueGeneratedNever();

            builder.Property(m => m.CustomerId)
                .IsRequired();

            builder.Property(m => m.PlanId)
                .IsRequired();

            builder.Property(m => m.DiscountPercent)
                .IsRequired();

            builder.Property(m => m.FreeShipping)
                .IsRequired();

            builder.OwnsOne(m => m.PlanTitle, name =>
            {
                name.Property(x => x.Value)
                .HasMaxLength(150).
                IsRequired();
            });


            // ۱. نگاشت StartDate با Conversion
            builder.Property(m => m.StartDate)
                .HasConversion(
                    d => d.Value,
                    v => new DomainDate(v))
                .HasColumnName("StartDate")
                .HasColumnType("datetime2")
                .IsRequired();

            // ۲. نگاشت EndDate با Conversion
            builder.Property(m => m.EndDate)
                .HasConversion(
                    d => d.Value,
                    v => new DomainDate(v))
                .HasColumnName("EndDate")
                .HasColumnType("datetime2")
                .IsRequired();

            // ۳. وضعیت
            builder.Property(m => m.Status)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            // ۴. ایندکس‌ها
            builder.HasIndex(m => new { m.CustomerId, m.Status });

            builder.HasIndex(m => new { m.Status, m.EndDate })
                .HasFilter("[Status] = 'Active'");
        }
    }
}
