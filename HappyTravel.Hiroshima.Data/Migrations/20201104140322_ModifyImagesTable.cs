using HappyTravel.Hiroshima.Common.Models.Images;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class ModifyImagesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LargeImageKey",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "LargeImageUri",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "OriginalContentType",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "OriginalName",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "SmallImageKey",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "SmallImageUri",
                table: "Images");

            migrationBuilder.AddColumn<ImageDetails>(
                name: "MainImage",
                table: "Images",
                type: "jsonb",
                nullable: false);

            migrationBuilder.AddColumn<OriginalImageDetails>(
                name: "OriginalImageDetails",
                table: "Images",
                type: "jsonb",
                nullable: false);

            migrationBuilder.AddColumn<ImageDetails>(
                name: "SmallImage",
                table: "Images",
                type: "jsonb",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainImage",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "OriginalImageDetails",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "SmallImage",
                table: "Images");

            migrationBuilder.AddColumn<string>(
                name: "LargeImageKey",
                table: "Images",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LargeImageUri",
                table: "Images",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OriginalContentType",
                table: "Images",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OriginalName",
                table: "Images",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SmallImageKey",
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
    }
}
