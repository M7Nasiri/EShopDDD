using _01.Domain.Entities.Aggregates.CategoryAgg;
using _01.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Infrastructure.PersistentEFCore.CategoryAgg
{

    public sealed class CategoryConfiguration
    : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedNever();


            builder.Property(x => x.ParentCategoryId).IsRequired(false);


            builder.HasOne<Category>()
                .WithMany()
                .HasForeignKey(x => x.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.HasIndex(x => x.ParentCategoryId);

            builder.Property(x => x.Name)
                .HasConversion(n => n.Value, v => new Name(v))
                .HasColumnName("Name")
                .HasMaxLength(150)
                .IsRequired();
        }
    }
}
