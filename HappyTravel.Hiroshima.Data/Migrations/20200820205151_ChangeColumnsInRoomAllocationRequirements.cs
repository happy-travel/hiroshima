using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class ChangeColumnsInRoomAllocationRequirements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "RoomAllocationRequirements");

            migrationBuilder.DropColumn(
                name: "MinimumStayNights",
                table: "RoomAllocationRequirements");

            migrationBuilder.DropColumn(
                name: "ReleasePeriod",
                table: "RoomAllocationRequirements");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "RoomAllocationRequirements");

            migrationBuilder.AlterColumn<int>(
                name: "Allotment",
                table: "RoomAllocationRequirements",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinimumLengthOfStay",
                table: "RoomAllocationRequirements",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReleaseDays",
                table: "RoomAllocationRequirements",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SeasonRangeId",
                table: "RoomAllocationRequirements",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimumLengthOfStay",
                table: "RoomAllocationRequirements");

            migrationBuilder.DropColumn(
                name: "ReleaseDays",
                table: "RoomAllocationRequirements");

            migrationBuilder.DropColumn(
                name: "SeasonRangeId",
                table: "RoomAllocationRequirements");

            migrationBuilder.AlterColumn<int>(
                name: "Allotment",
                table: "RoomAllocationRequirements",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "RoomAllocationRequirements",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "MinimumStayNights",
                table: "RoomAllocationRequirements",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReleasePeriod",
                table: "RoomAllocationRequirements",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "RoomAllocationRequirements",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
