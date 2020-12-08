using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class ChangeAccommodationCoordinatesType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Point>(
                name: "Coordinates",
                table: "Accommodations",
                type: "geography (point)",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geometry (point)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Point>(
                name: "Coordinates",
                table: "Accommodations",
                type: "geometry (point)",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geography (point)");
        }
    }
}
