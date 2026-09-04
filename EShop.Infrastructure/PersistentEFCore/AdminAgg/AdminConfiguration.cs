using _01.Domain.Entities.Aggregates.AdminAgg;
using _01.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Infrastructure.PersistentEFCore.AdminAgg
{
    internal class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.ToTable("Admins");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedNever();


            builder.Property(x => x.FullName)
            .HasConversion(n => n.Value, v => new Name(v))
            .HasColumnName("FullName")
            .HasMaxLength(200)
            .IsRequired();
        }
    }
}
