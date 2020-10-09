using Microsoft.EntityFrameworkCore.Migrations;

namespace MotoShop.Data.Migrations
{
    public partial class changedecimaltofloatinShopItemModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "ShopItem",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldMaxLength: 20);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "ShopItem",
                type: "decimal(18,2)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(float),
                oldMaxLength: 20);
        }
    }
}
