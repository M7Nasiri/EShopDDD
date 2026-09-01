//namespace EShop.Infrastructure.PersistentEFCore.CartAgg
//{
//    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
//{
//    public void Configure(EntityTypeBuilder<CartItem> builder)
//    {
//        builder.ToTable("CartItems");

//        builder.HasKey(ci => ci.Id);
//        builder.Property(ci => ci.Id)
//            .HasConversion(id => id.Value, value => new Id(value));

//        // کلید خارجی مرجع کالا
//        builder.Property(ci => ci.ProductId)
//            .HasConversion(id => id.Value, value => new Id(value))
//            .IsRequired();

//        // نگاشت Value Object تعداد (Quantity)
//        builder.Property(ci => ci.Quantity)
//            .HasConversion(q => q.Value, value => new Quantity(value))
//            .IsRequired();

//        // ایندکس ترکیبی یونیک روی (CartId, ProductId)
//        // این ایندکس تضمین می‌کند که یک کالا دو بار در جدول به صورت ردیف مجزا برای یک سبد ثبت نشود
//        builder.HasIndex("CartId", nameof(CartItem.ProductId))
//            .IsUnique();
//    }
//}
//}
