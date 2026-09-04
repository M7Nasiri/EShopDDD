using _01.Domain.Entities.Aggregates.ProductAgg;
using _01.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Infrastructure.PersistentEFCore.ProductAgg
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedNever();


            builder.Property(p => p.Name)
                .HasConversion(name => name.Value, value => new Name(value))
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasConversion(desc => desc.Value, value => new Description(value))
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(p => p.Stock)
                .HasConversion(stock => stock.Value, value => new Quantity(value))
                .IsRequired();


            builder.Property(p => p.UnitPrice)
                      .HasConversion(m => m.Amount, v => new Money(v))
                      .HasColumnName("UnitPrice")
                      .HasPrecision(18, 2)
                      .IsRequired();

            builder.Property(p => p.CategoryId)
                .IsRequired();

            builder.Property(p => p.CreatedByUserId)
                .IsRequired();
         


            builder.Property(p => p.ImageName)
               .HasConversion(
                   img => img != null ? img.Value : null,
                   str => !string.IsNullOrEmpty(str) ? new Name(str) : null)
               .HasMaxLength(200)
               .IsRequired(false);


            builder.HasIndex(p => p.CategoryId);
            builder.HasIndex(p => p.Name);


            builder.Navigation(p => p.Attributes)
                .HasField("_attributes")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsMany(p => p.Attributes, attr =>
            {
                attr.ToTable("ProductAttributes");

                attr.WithOwner()
                    .HasForeignKey("ProductId");

                attr.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                attr.HasKey("Id");

                attr.Property(a => a.Key)
                    .HasMaxLength(100)
                    .IsRequired();

                attr.Property(a => a.Value)
                    .HasMaxLength(500)
                    .IsRequired();

                attr.HasIndex("ProductId", nameof(ProductAttribute.Key)).IsUnique();
            });

            builder.Navigation(p => p.Images)
                .HasField("_images")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsMany(p => p.Images, img =>
            {
                img.ToTable("ProductImages");

                img.WithOwner()
                    .HasForeignKey(i => i.ProductId);

                img.HasKey(i => i.Id);

                img.Property(i => i.Id)
                    .ValueGeneratedNever();

                img.Property(i => i.ImageName)
                    .HasConversion(
                        name => name.Value,
                        value => new Name(value))
                    .HasMaxLength(200)
                    .IsRequired();

                img.Property(i => i.Sequence)
                    .IsRequired();

                img.HasIndex(i => new
                {
                    i.ProductId,
                    i.Sequence
                }).IsUnique();
            });

        }
    }
}
