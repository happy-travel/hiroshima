using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class ModifyAvailabilityRestrictionstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "RoomAvailabilityRestrictions");

            migrationBuilder.DropColumn(
                name: "Restrictions",
                table: "RoomAvailabilityRestrictions");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "RoomAvailabilityRestrictions");

            migrationBuilder.AddColumn<int>(
                name: "ContractId",
                table: "RoomAvailabilityRestrictions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "RoomAvailabilityRestrictions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Restriction",
                table: "RoomAvailabilityRestrictions",
                nullable: false,
                defaultValue: 3);

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate",
                table: "RoomAvailabilityRestrictions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_RoomPromotionalOffers_ContractId",
                table: "RoomPromotionalOffers",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomAvailabilityRestrictions_ContractId",
                table: "RoomAvailabilityRestrictions",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomAvailabilityRestrictions_Restriction",
                table: "RoomAvailabilityRestrictions",
                column: "Restriction");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomAvailabilityRestrictions_Contracts_ContractId",
                table: "RoomAvailabilityRestrictions",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomPromotionalOffers_Contracts_ContractId",
                table: "RoomPromotionalOffers",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seasons_Contracts_ContractId",
                table: "Seasons",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomAvailabilityRestrictions_Contracts_ContractId",
                table: "RoomAvailabilityRestrictions");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomPromotionalOffers_Contracts_ContractId",
                table: "RoomPromotionalOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_Seasons_Contracts_ContractId",
                table: "Seasons");

            migrationBuilder.DropIndex(
                name: "IX_RoomPromotionalOffers_ContractId",
                table: "RoomPromotionalOffers");

            migrationBuilder.DropIndex(
                name: "IX_RoomAvailabilityRestrictions_ContractId",
                table: "RoomAvailabilityRestrictions");

            migrationBuilder.DropIndex(
                name: "IX_RoomAvailabilityRestrictions_Restriction",
                table: "RoomAvailabilityRestrictions");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "RoomAvailabilityRestrictions");

            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "RoomAvailabilityRestrictions");

            migrationBuilder.DropColumn(
                name: "Restriction",
                table: "RoomAvailabilityRestrictions");

            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "RoomAvailabilityRestrictions");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "RoomAvailabilityRestrictions",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Restrictions",
                table: "RoomAvailabilityRestrictions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "RoomAvailabilityRestrictions",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
