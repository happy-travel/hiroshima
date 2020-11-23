using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class RenameBookingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_ContractManagers_ContractManagerId",
                table: "Booking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Booking",
                table: "Booking");

            migrationBuilder.RenameTable(
                name: "Booking",
                newName: "BookingOrders");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_ContractManagerId",
                table: "BookingOrders",
                newName: "IX_BookingOrders_ContractManagerId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Modified",
                table: "BookingOrders",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "BookingOrders",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "BookingOrders",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingOrders",
                table: "BookingOrders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingOrders_ContractManagers_ContractManagerId",
                table: "BookingOrders",
                column: "ContractManagerId",
                principalTable: "ContractManagers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingOrders_ContractManagers_ContractManagerId",
                table: "BookingOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingOrders",
                table: "BookingOrders");

            migrationBuilder.RenameTable(
                name: "BookingOrders",
                newName: "Booking");

            migrationBuilder.RenameIndex(
                name: "IX_BookingOrders_ContractManagerId",
                table: "Booking",
                newName: "IX_Booking_ContractManagerId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Modified",
                table: "Booking",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Booking",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Booking",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Booking",
                table: "Booking",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_ContractManagers_ContractManagerId",
                table: "Booking",
                column: "ContractManagerId",
                principalTable: "ContractManagers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
