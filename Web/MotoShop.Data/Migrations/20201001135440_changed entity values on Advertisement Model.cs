using Microsoft.EntityFrameworkCore.Migrations;

namespace MotoShop.Data.Migrations
{
    public partial class changedentityvaluesonAdvertisementModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopItem_AspNetUsers_OwnerID",
                table: "ShopItem");

            migrationBuilder.DropIndex(
                name: "IX_ShopItem_OwnerID",
                table: "ShopItem");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerID",
                table: "ShopItem",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "ShopItem",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "OwnerID",
                table: "ShopItem",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopItem_OwnerID",
                table: "ShopItem",
                column: "OwnerID");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopItem_AspNetUsers_OwnerID",
                table: "ShopItem",
                column: "OwnerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
