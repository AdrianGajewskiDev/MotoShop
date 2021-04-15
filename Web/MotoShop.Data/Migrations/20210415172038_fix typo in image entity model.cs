using Microsoft.EntityFrameworkCore.Migrations;

namespace MotoShop.Data.Migrations
{
    public partial class fixtypoinimageentitymodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Advertisements_AdvertisementID",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "AdvrtisementID",
                table: "Images");

            migrationBuilder.AlterColumn<int>(
                name: "AdvertisementID",
                table: "Images",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Advertisements_AdvertisementID",
                table: "Images",
                column: "AdvertisementID",
                principalTable: "Advertisements",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Advertisements_AdvertisementID",
                table: "Images");

            migrationBuilder.AlterColumn<int>(
                name: "AdvertisementID",
                table: "Images",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "AdvrtisementID",
                table: "Images",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Advertisements_AdvertisementID",
                table: "Images",
                column: "AdvertisementID",
                principalTable: "Advertisements",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
