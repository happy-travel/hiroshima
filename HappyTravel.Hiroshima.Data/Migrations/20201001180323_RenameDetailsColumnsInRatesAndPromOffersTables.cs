using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class RenameDetailsColumnsInRatesAndPromOffersTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Details",
                table: "RoomRates",
                newName: "Remarks");

            migrationBuilder.RenameColumn(
                name: "Details",
                table: "RoomPromotionalOffers",
                newName: "Remarks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Remarks",
                table: "RoomRates",
                newName: "Details");

            migrationBuilder.RenameColumn(
                name: "Remarks",
                table: "RoomPromotionalOffers",
                newName: "Details");
        }
    }
}
