using _01.Domain.Entities.Aggregates.AdminAgg;
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
            builder.Property(c => c.Id);


            builder.OwnsOne(x => x.FullName, name =>
            {
                name.Property(x => x.Value)
                    .HasColumnName("FullName")
                    .HasMaxLength(300)
                    .IsRequired();
            });
        }
    }
}
