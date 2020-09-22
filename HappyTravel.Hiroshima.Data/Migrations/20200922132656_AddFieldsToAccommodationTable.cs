using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class AddFieldsToAccommodationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuildYear",
                table: "Accommodations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Floors",
                table: "Accommodations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildYear",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "Floors",
                table: "Accommodations");
        }
    }
}
