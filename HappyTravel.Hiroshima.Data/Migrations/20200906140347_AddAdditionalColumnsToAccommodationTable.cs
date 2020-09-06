using System.Text.Json;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class AddAdditionalColumnsToAccommodationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<JsonDocument>(
                name: "LeisureAndSports",
                table: "Accommodations",
                type: "jsonb",
                nullable: false);

            migrationBuilder.AddColumn<RateOptions>(
                name: "RateOptions",
                table: "Accommodations",
                type: "jsonb",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Accommodations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeisureAndSports",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "RateOptions",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Accommodations");
        }
    }
}
