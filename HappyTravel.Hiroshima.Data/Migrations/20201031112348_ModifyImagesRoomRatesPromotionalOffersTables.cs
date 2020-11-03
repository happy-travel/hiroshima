using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class ModifyImagesRoomRatesPromotionalOffersTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Remarks",
                table: "RoomRates",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Remarks",
                table: "RoomPromotionalOffers",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "LargeImageUri",
                table: "Images",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SmallImageUri",
                table: "Images",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LargeImageUri",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "SmallImageUri",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "RoomRates",
                newName: "Remarks");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "RoomPromotionalOffers",
                newName: "Remarks");
        }
    }
}
