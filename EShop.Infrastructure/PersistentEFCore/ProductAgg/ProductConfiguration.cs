using _01.Domain.Entities.Aggregates.ProductAgg;
using _01.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.PersistentEFCore.ProductAgg
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id);


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

            // کانفیگ Value Object پول (واحد و مقدار)
            builder.OwnsOne(p => p.UnitPrice, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("UnitPrice_Amount")
                    .HasPrecision(18, 2)
                    .IsRequired();              
            });

            builder.Property(p => p.CategoryId)
                .IsRequired();

            builder.Property(p => p.CreatedByUserId)
                .IsRequired();

            // نگاشت کالکشن خصوصیات (Attributes)
            builder.OwnsMany(p => p.Attributes, attr =>
            {
                attr.ToTable("ProductAttributes");
                //attr.WithOwner().HasForeignKey("ProductId");
                //attr.Property<int>("Id").ValueGeneratedOnAdd();
                //attr.HasKey("Id");

                attr.Property(a => a.Key).HasMaxLength(100).IsRequired();
                attr.Property(a => a.Value).HasMaxLength(500).IsRequired();
            });

            builder.OwnsMany(b => b.Images, option =>
            {
                option.ToTable("Images");
                option.Property(b => b.ImageName)
                    .IsRequired()
                    .HasMaxLength(100);
            });


            // دسترسی مستقیم EF به فیلد خصوصی _attributes
            builder.Metadata.FindNavigation(nameof(Product.Attributes))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasIndex(p => p.CategoryId);
            builder.HasIndex(p => p.Name);
        }
    }
}
