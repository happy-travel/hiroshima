using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class ModifyAccommodationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "DiscountPercent",
                table: "RoomPromotionalOffers",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Accommodations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Accommodations");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountPercent",
                table: "RoomPromotionalOffers",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }
    }
}
