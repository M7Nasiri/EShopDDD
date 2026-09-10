using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.Infrastructure.Migrations.Shop
{
    /// <inheritdoc />
    public partial class Add_Comment_Relation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ۱. ارتباط کامنت با مشتری
            migrationBuilder.AddForeignKey(
                name: "FK_ProductComments_Customers_CustomerId",
                table: "ProductComments",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // ۲. ارتباط کامنت با کالا
            migrationBuilder.AddForeignKey(
                name: "FK_ProductComments_Products_ProductId",
                table: "ProductComments",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade); // در صورت حذف کالا، نظرات آن هم حذف شوند

            migrationBuilder.CreateIndex(
                name: "IX_ProductComments_CustomerId",
                table: "ProductComments",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductComments_Customers_CustomerId",
                table: "ProductComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductComments_Products_ProductId",
                table: "ProductComments");

            migrationBuilder.DropIndex(
                name: "IX_ProductComments_CustomerId",
                table: "ProductComments");
        }
    }
}
