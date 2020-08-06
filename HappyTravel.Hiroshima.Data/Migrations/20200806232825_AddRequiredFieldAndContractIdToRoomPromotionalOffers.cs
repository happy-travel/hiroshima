using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class AddRequiredFieldAndContractIdToRoomPromotionalOffers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<JsonDocument>(
                name: "Pictures",
                table: "Rooms",
                nullable: false,
                oldClrType: typeof(JsonDocument),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MealPlan",
                table: "RoomRates",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<JsonDocument>(
                name: "Details",
                table: "RoomRates",
                nullable: false,
                oldClrType: typeof(JsonDocument),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AlterColumn<JsonDocument>(
                name: "Details",
                table: "RoomPromotionalOffers",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(JsonDocument),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BookingCode",
                table: "RoomPromotionalOffers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "ContractId",
                table: "RoomPromotionalOffers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<JsonDocument>(
                name: "Zone",
                table: "Locations",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'::json",
                oldClrType: typeof(JsonDocument),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AlterColumn<JsonDocument>(
                name: "Pictures",
                table: "Accommodations",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(JsonDocument),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AlterColumn<JsonDocument>(
                name: "AdditionalInfo",
                table: "Accommodations",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'::json",
                oldClrType: typeof(JsonDocument),
                oldType: "jsonb",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "RoomPromotionalOffers");

            migrationBuilder.AlterColumn<JsonDocument>(
                name: "Pictures",
                table: "Rooms",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(JsonDocument));

            migrationBuilder.AlterColumn<string>(
                name: "MealPlan",
                table: "RoomRates",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<JsonDocument>(
                name: "Details",
                table: "RoomRates",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(JsonDocument));

            migrationBuilder.AlterColumn<JsonDocument>(
                name: "Details",
                table: "RoomPromotionalOffers",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(JsonDocument),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<string>(
                name: "BookingCode",
                table: "RoomPromotionalOffers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<JsonDocument>(
                name: "Zone",
                table: "Locations",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(JsonDocument),
                oldType: "jsonb",
                oldDefaultValueSql: "'{}'::json");

            migrationBuilder.AlterColumn<JsonDocument>(
                name: "Pictures",
                table: "Accommodations",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(JsonDocument),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<JsonDocument>(
                name: "AdditionalInfo",
                table: "Accommodations",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(JsonDocument),
                oldType: "jsonb",
                oldDefaultValueSql: "'{}'::json");
        }
    }
}
