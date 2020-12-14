using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class RemovePicturesFromAccommodationsAndRooms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pictures",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Pictures",
                table: "Accommodations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<MultiLanguage<List<Picture>>>(
                name: "Pictures",
                table: "Rooms",
                type: "jsonb",
                nullable: false);

            migrationBuilder.AddColumn<MultiLanguage<List<Picture>>>(
                name: "Pictures",
                table: "Accommodations",
                type: "jsonb",
                nullable: false);
        }
    }
}
