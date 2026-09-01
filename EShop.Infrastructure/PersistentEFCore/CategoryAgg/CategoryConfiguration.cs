using _01.Domain.Entities.Aggregates.CategoryAgg;
using _01.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.PersistentEFCore.CategoryAgg
{

    public sealed class CategoryConfiguration
    : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);


            builder.Property(x => x.ParentCategoryId);


            builder.HasOne<Category>()
                .WithMany()
                .HasForeignKey(x => x.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.ParentCategoryId);

            builder.OwnsOne(x => x.Name, name =>
            {
                name.Property(x => x.Value)
                    .HasColumnName("Name")
                    .HasMaxLength(150)
                    .IsRequired();
            });
        }
    }
}
