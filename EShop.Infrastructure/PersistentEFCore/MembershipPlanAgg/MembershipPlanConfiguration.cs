using _01.Domain.Entities.Aggregates.MembershipPlanAgg;
using _01.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Infrastructure.PersistentEFCore.MembershipPlanAgg
{
    public class MembershipPlanConfiguration : IEntityTypeConfiguration<MembershipPlan>
    {
        public void Configure(EntityTypeBuilder<MembershipPlan> builder)
        {
            builder.ToTable("MembershipPlans");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedNever();


            builder.Property(p => p.Name)
                .HasConversion(n => n.Value, v => new Name(v))
                .HasColumnName("Name")
                .HasMaxLength(150)
                .IsRequired();

            builder.HasIndex(p => p.Name).IsUnique();

            builder.Property(p => p.Price)
                .HasConversion(m => m.Amount, v => new Money(v))
                .HasColumnName("Price")
                .HasPrecision(18, 2)
                .IsRequired();

           
            builder.Property(p => p.DurationInDays)
                .HasConversion(d => d.Days, v => new MembershipDuration(v))
                .HasColumnName("DurationInDays")
                .IsRequired();

            builder.Property(p => p.DiscountPercent)
                .IsRequired();

            builder.Property(p => p.FreeShipping)
                .IsRequired();

            builder.Property(p => p.IsActive)
                .IsRequired();
        }
    }
}
