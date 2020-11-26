using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Images;
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

            migrationBuilder.DropColumn(
                name: "MainImage",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "SmallImage",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "AccommodationId",
                table: "Images",
                newName: "ReferenceId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_AccommodationId",
                table: "Images",
                newName: "IX_Images_ReferenceId");

            migrationBuilder.AddColumn<List<SlimImage>>(
                name: "Images",
                table: "Rooms",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'[]'::json");

            migrationBuilder.AddColumn<int>(
                name: "ImageType",
                table: "Images",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<ImageKeys>(
                name: "Keys",
                table: "Images",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'::json");

            migrationBuilder.AddColumn<List<SlimImage>>(
                name: "Images",
                table: "Accommodations",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'[]'::json");

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
                name: "Keys",
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

            migrationBuilder.AddColumn<ImageDetails>(
                name: "MainImage",
                table: "Images",
                type: "jsonb",
                nullable: false);

            migrationBuilder.AddColumn<ImageDetails>(
                name: "SmallImage",
                table: "Images",
                type: "jsonb",
                nullable: false);

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
