using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class AddSeasonsAndRemoveForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractAccommodationRelations_Accommodations_Accommodation~",
                table: "ContractAccommodationRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractAccommodationRelations_Contracts_ContractId",
                table: "ContractAccommodationRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomAllocationRequirements_Rooms_RoomId",
                table: "RoomAllocationRequirements");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomAvailabilityRestrictions_Rooms_RoomId",
                table: "RoomAvailabilityRestrictions");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomPromotionalOffers_Rooms_RoomId",
                table: "RoomPromotionalOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomRates_Rooms_RoomId",
                table: "RoomRates");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Accommodations_AccommodationId",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Locations",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "RoomRates");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "RoomRates");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "CancellationPolicies");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "CancellationPolicies");

            migrationBuilder.AddColumn<int>(
                name: "SeasonId",
                table: "RoomRates",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Locations",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "SeasonId",
                table: "CancellationPolicies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Locations",
                table: "Locations",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    ContractId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomRates_SeasonId",
                table: "RoomRates",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_CountryCode",
                table: "Locations",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_CancellationPolicies_RoomId",
                table: "CancellationPolicies",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_CancellationPolicies_SeasonId",
                table: "CancellationPolicies",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_ContractId",
                table: "Seasons",
                column: "ContractId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropIndex(
                name: "IX_RoomRates_SeasonId",
                table: "RoomRates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Locations",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_CountryCode",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_CancellationPolicies_RoomId",
                table: "CancellationPolicies");

            migrationBuilder.DropIndex(
                name: "IX_CancellationPolicies_SeasonId",
                table: "CancellationPolicies");

            migrationBuilder.DropColumn(
                name: "SeasonId",
                table: "RoomRates");

            migrationBuilder.DropColumn(
                name: "SeasonId",
                table: "CancellationPolicies");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "RoomRates",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "RoomRates",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Locations",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "CancellationPolicies",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "CancellationPolicies",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Locations",
                table: "Locations",
                column: "CountryCode");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractAccommodationRelations_Accommodations_Accommodation~",
                table: "ContractAccommodationRelations",
                column: "AccommodationId",
                principalTable: "Accommodations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractAccommodationRelations_Contracts_ContractId",
                table: "ContractAccommodationRelations",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomAllocationRequirements_Rooms_RoomId",
                table: "RoomAllocationRequirements",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomAvailabilityRestrictions_Rooms_RoomId",
                table: "RoomAvailabilityRestrictions",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomPromotionalOffers_Rooms_RoomId",
                table: "RoomPromotionalOffers",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomRates_Rooms_RoomId",
                table: "RoomRates",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Accommodations_AccommodationId",
                table: "Rooms",
                column: "AccommodationId",
                principalTable: "Accommodations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
