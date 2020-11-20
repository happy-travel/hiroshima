using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class ChangeTableImagesAddImageType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Accommodations_AccommodationId",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "AccommodationId",
                table: "Images",
                newName: "ReferenceId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_AccommodationId",
                table: "Images",
                newName: "IX_Images_ReferenceId");

            migrationBuilder.AddColumn<JsonDocument[]>(
                name: "Images",
                table: "Rooms",
                type: "jsonb[]",
                nullable: false,
                defaultValue: new JsonDocument[0]);

            migrationBuilder.AddColumn<int>(
                name: "ImageType",
                table: "Images",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<JsonDocument[]>(
                name: "Images",
                table: "Accommodations",
                type: "jsonb[]",
                nullable: false,
                defaultValue: new JsonDocument[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Images_ImageType",
                table: "Images",
                column: "ImageType");

            migrationBuilder.CreateIndex(
                name: "IX_Amenities_LanguageCode",
                table: "Amenities",
                column: "LanguageCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Images_ImageType",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Amenities_LanguageCode",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "Images",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "ImageType",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Images",
                table: "Accommodations");

            migrationBuilder.RenameColumn(
                name: "ReferenceId",
                table: "Images",
                newName: "AccommodationId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_ReferenceId",
                table: "Images",
                newName: "IX_Images_AccommodationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Accommodations_AccommodationId",
                table: "Images",
                column: "AccommodationId",
                principalTable: "Accommodations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
