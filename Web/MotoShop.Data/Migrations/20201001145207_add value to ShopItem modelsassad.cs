using Microsoft.EntityFrameworkCore.Migrations;

namespace MotoShop.Data.Migrations
{
    public partial class addvaluetoShopItemmodelsassad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopItem_Advertisements_AdvertisementID",
                table: "ShopItem");

            migrationBuilder.DropIndex(
                name: "IX_ShopItem_AdvertisementID",
                table: "ShopItem");

            migrationBuilder.DropColumn(
                name: "AdvertisementID",
                table: "ShopItem");

            migrationBuilder.AddColumn<int>(
                name: "ShopItemID",
                table: "Advertisements",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_ShopItemID",
                table: "Advertisements",
                column: "ShopItemID");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_ShopItem_ShopItemID",
                table: "Advertisements",
                column: "ShopItemID",
                principalTable: "ShopItem",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_ShopItem_ShopItemID",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_ShopItemID",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "ShopItemID",
                table: "Advertisements");

            migrationBuilder.AddColumn<int>(
                name: "AdvertisementID",
                table: "ShopItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShopItem_AdvertisementID",
                table: "ShopItem",
                column: "AdvertisementID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopItem_Advertisements_AdvertisementID",
                table: "ShopItem",
                column: "AdvertisementID",
                principalTable: "Advertisements",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
