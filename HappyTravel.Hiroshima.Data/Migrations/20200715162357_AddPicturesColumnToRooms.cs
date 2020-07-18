using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class AddPicturesColumnToRooms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<JsonDocument>(
                name: "Pictures",
                table: "Rooms",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PropertyType",
                table: "Accommodations",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pictures",
                table: "Rooms");

            migrationBuilder.AlterColumn<int>(
                name: "PropertyType",
                table: "Accommodations",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
