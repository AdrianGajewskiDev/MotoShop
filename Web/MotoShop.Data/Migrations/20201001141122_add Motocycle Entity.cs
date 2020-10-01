using Microsoft.EntityFrameworkCore.Migrations;

namespace MotoShop.Data.Migrations
{
    public partial class addMotocycleEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopItem_AspNetUsers_ApplicationUserId",
                table: "ShopItem");

            migrationBuilder.DropIndex(
                name: "IX_ShopItem_ApplicationUserId",
                table: "ShopItem");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "ShopItem");

            migrationBuilder.AddColumn<string>(
                name: "MotocycleBrand",
                table: "ShopItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotocycleModel",
                table: "ShopItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemType",
                table: "ShopItem",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MotocycleBrand",
                table: "ShopItem");

            migrationBuilder.DropColumn(
                name: "MotocycleModel",
                table: "ShopItem");

            migrationBuilder.DropColumn(
                name: "ItemType",
                table: "ShopItem");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "ShopItem",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopItem_ApplicationUserId",
                table: "ShopItem",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopItem_AspNetUsers_ApplicationUserId",
                table: "ShopItem",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
