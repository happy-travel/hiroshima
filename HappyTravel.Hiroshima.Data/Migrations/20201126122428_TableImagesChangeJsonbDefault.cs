using System.Collections.Generic;
using System.Text.Json;
using HappyTravel.Hiroshima.Common.Models.Images;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class TableImagesChangeJsonbDefault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<SlimImage>>(
                name: "Images",
                table: "Rooms",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'[]'::jsonb",
                oldClrType: typeof(List<SlimImage>),
                oldType: "jsonb",
                oldDefaultValueSql: "'[]'::json");

            migrationBuilder.AlterColumn<ImageKeys>(
                name: "Keys",
                table: "Images",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'::jsonb",
                oldClrType: typeof(ImageKeys),
                oldType: "jsonb",
                oldDefaultValueSql: "'{}'::json");

            migrationBuilder.AlterColumn<List<SlimImage>>(
                name: "Images",
                table: "Accommodations",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'[]'::jsonb",
                oldClrType: typeof(List<SlimImage>),
                oldType: "jsonb",
                oldDefaultValueSql: "'[]'::json");

            migrationBuilder.AlterColumn<JsonDocument>(
                name: "AdditionalInfo",
                table: "Accommodations",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'::jsonb",
                oldClrType: typeof(JsonDocument),
                oldType: "jsonb",
                oldDefaultValueSql: "'{}'::json");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<SlimImage>>(
                name: "Images",
                table: "Rooms",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'[]'::json",
                oldClrType: typeof(List<SlimImage>),
                oldType: "jsonb",
                oldDefaultValueSql: "'[]'::jsonb");

            migrationBuilder.AlterColumn<ImageKeys>(
                name: "Keys",
                table: "Images",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'::json",
                oldClrType: typeof(ImageKeys),
                oldType: "jsonb",
                oldDefaultValueSql: "'{}'::jsonb");

            migrationBuilder.AlterColumn<List<SlimImage>>(
                name: "Images",
                table: "Accommodations",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'[]'::json",
                oldClrType: typeof(List<SlimImage>),
                oldType: "jsonb",
                oldDefaultValueSql: "'[]'::jsonb");

            migrationBuilder.AlterColumn<JsonDocument>(
                name: "AdditionalInfo",
                table: "Accommodations",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'::json",
                oldClrType: typeof(JsonDocument),
                oldType: "jsonb",
                oldDefaultValueSql: "'{}'::jsonb");
        }
    }
}
