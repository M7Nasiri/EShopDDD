using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.Infrastructure.Migrations.Shop
{
    /// <inheritdoc />
    public partial class addConstraintAndIndexOnCartTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"
                ALTER TABLE Carts
                ADD CONSTRAINT CK_Carts_Owner
                CHECK (
                    (CustomerId IS NOT NULL AND GuestId IS NULL)
                    OR
                    (CustomerId IS NULL AND GuestId IS NOT NULL))
            ");

            migrationBuilder.Sql(@"
                CREATE UNIQUE INDEX UX_Carts_Active_Customer
                ON Carts(CustomerId)
                WHERE CustomerId IS NOT NULL
                AND Status = 1;
            ");

            migrationBuilder.Sql(@"
                CREATE UNIQUE INDEX UX_Carts_Active_Guest
                ON Carts(GuestId)
                WHERE GuestId IS NOT NULL
                  AND Status = 1;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Carts");

        }
    }
}
