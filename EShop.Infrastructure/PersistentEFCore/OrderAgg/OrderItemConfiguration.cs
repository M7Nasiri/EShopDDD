using _01.Domain.Entities.Aggregates.OrderAgg;
using _01.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.PersistentEFCore.OrderAgg
{
    //public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    //{
    //    public void Configure(EntityTypeBuilder<OrderItem> builder)
    //    {
    //        builder.ToTable("OrderItems");

    //        builder.HasKey(i => i.Id);

    //        builder.Property(i => i.Id)
    //            .HasConversion(id => id.Value, value => new Id(value))
    //            .ValueGeneratedNever();

    //        builder.Property(i => i.ProductId)
    //            .HasConversion(id => id.Value, value => new Id(value))
    //            .IsRequired();

    //        // نگاشت Value Object تعداد (Quantity)
    //        builder.OwnsOne(i => i.Quantity, q =>
    //        {
    //            q.Property(x => x.Value)
    //                .HasColumnName("Quantity")
    //                .IsRequired();
    //        });

    //        // نگاشت Value Object قیمت (Money)
    //        builder.OwnsOne(i => i.UnitPrice, m =>
    //        {
    //            m.Property(x => x.Amount)
    //                .HasColumnName("UnitPrice")
    //                .HasPrecision(18, 2)
    //                .IsRequired();
    //        });
    //    }
    //}
}
