using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.Infrastructure.Migrations.Shop
{
    /// <inheritdoc />
    public partial class Add_Cart_Relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                Alter Table Carts
                Add Constraint FK_Carts_Customers_CustomerId
                Foreign Key (CustomerId) References Customers(Id)
                On Delete Set Null;
            ");

            migrationBuilder.Sql(@"
                Alter Table CartItems
                Add Constraint FK_CartItems_Products_ProductId
                Foreign Key (ProductId) References Products(Id)
                On Delete Cascade;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
