using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class RemoveCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_ContractManagers_ContractManagerId",
                table: "Accommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_Locations_LocationId",
                table: "Accommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_CancellationPolicies_Rooms_RoomId",
                table: "CancellationPolicies");

            migrationBuilder.DropForeignKey(
                name: "FK_CancellationPolicies_Seasons_SeasonId",
                table: "CancellationPolicies");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_ContractManagers_ContractManagerId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Accommodations_AccommodationId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Countries_CountryCode",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomAllocationRequirements_Rooms_RoomId",
                table: "RoomAllocationRequirements");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomAllocationRequirements_SeasonRanges_SeasonRangeId",
                table: "RoomAllocationRequirements");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomAvailabilityRestrictions_Contracts_ContractId",
                table: "RoomAvailabilityRestrictions");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomAvailabilityRestrictions_Rooms_RoomId",
                table: "RoomAvailabilityRestrictions");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomPromotionalOffers_Contracts_ContractId",
                table: "RoomPromotionalOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomPromotionalOffers_Rooms_RoomId",
                table: "RoomPromotionalOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomRates_Rooms_RoomId",
                table: "RoomRates");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomRates_Seasons_SeasonId",
                table: "RoomRates");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Accommodations_AccommodationId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_SeasonRanges_Seasons_SeasonId",
                table: "SeasonRanges");

            migrationBuilder.DropForeignKey(
                name: "FK_Seasons_Contracts_ContractId",
                table: "Seasons");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_ContractManagers_ContractManagerId",
                table: "Accommodations",
                column: "ContractManagerId",
                principalTable: "ContractManagers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_Locations_LocationId",
                table: "Accommodations",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CancellationPolicies_Rooms_RoomId",
                table: "CancellationPolicies",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CancellationPolicies_Seasons_SeasonId",
                table: "CancellationPolicies",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_ContractManagers_ContractManagerId",
                table: "Contracts",
                column: "ContractManagerId",
                principalTable: "ContractManagers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Contracts_ContractId",
                table: "Documents",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Accommodations_AccommodationId",
                table: "Images",
                column: "AccommodationId",
                principalTable: "Accommodations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Countries_CountryCode",
                table: "Locations",
                column: "CountryCode",
                principalTable: "Countries",
                principalColumn: "Code",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomAllocationRequirements_Rooms_RoomId",
                table: "RoomAllocationRequirements",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomAllocationRequirements_SeasonRanges_SeasonRangeId",
                table: "RoomAllocationRequirements",
                column: "SeasonRangeId",
                principalTable: "SeasonRanges",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomAvailabilityRestrictions_Contracts_ContractId",
                table: "RoomAvailabilityRestrictions",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomAvailabilityRestrictions_Rooms_RoomId",
                table: "RoomAvailabilityRestrictions",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomPromotionalOffers_Contracts_ContractId",
                table: "RoomPromotionalOffers",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomPromotionalOffers_Rooms_RoomId",
                table: "RoomPromotionalOffers",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomRates_Rooms_RoomId",
                table: "RoomRates",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomRates_Seasons_SeasonId",
                table: "RoomRates",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Accommodations_AccommodationId",
                table: "Rooms",
                column: "AccommodationId",
                principalTable: "Accommodations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SeasonRanges_Seasons_SeasonId",
                table: "SeasonRanges",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Seasons_Contracts_ContractId",
                table: "Seasons",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_ContractManagers_ContractManagerId",
                table: "Accommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_Locations_LocationId",
                table: "Accommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_CancellationPolicies_Rooms_RoomId",
                table: "CancellationPolicies");

            migrationBuilder.DropForeignKey(
                name: "FK_CancellationPolicies_Seasons_SeasonId",
                table: "CancellationPolicies");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_ContractManagers_ContractManagerId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Contracts_ContractId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Accommodations_AccommodationId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Countries_CountryCode",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomAllocationRequirements_Rooms_RoomId",
                table: "RoomAllocationRequirements");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomAllocationRequirements_SeasonRanges_SeasonRangeId",
                table: "RoomAllocationRequirements");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomAvailabilityRestrictions_Contracts_ContractId",
                table: "RoomAvailabilityRestrictions");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomAvailabilityRestrictions_Rooms_RoomId",
                table: "RoomAvailabilityRestrictions");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomPromotionalOffers_Contracts_ContractId",
                table: "RoomPromotionalOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomPromotionalOffers_Rooms_RoomId",
                table: "RoomPromotionalOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomRates_Rooms_RoomId",
                table: "RoomRates");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomRates_Seasons_SeasonId",
                table: "RoomRates");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Accommodations_AccommodationId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_SeasonRanges_Seasons_SeasonId",
                table: "SeasonRanges");

            migrationBuilder.DropForeignKey(
                name: "FK_Seasons_Contracts_ContractId",
                table: "Seasons");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_ContractManagers_ContractManagerId",
                table: "Accommodations",
                column: "ContractManagerId",
                principalTable: "ContractManagers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_Locations_LocationId",
                table: "Accommodations",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CancellationPolicies_Rooms_RoomId",
                table: "CancellationPolicies",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CancellationPolicies_Seasons_SeasonId",
                table: "CancellationPolicies",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_ContractManagers_ContractManagerId",
                table: "Contracts",
                column: "ContractManagerId",
                principalTable: "ContractManagers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Accommodations_AccommodationId",
                table: "Images",
                column: "AccommodationId",
                principalTable: "Accommodations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Countries_CountryCode",
                table: "Locations",
                column: "CountryCode",
                principalTable: "Countries",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomAllocationRequirements_Rooms_RoomId",
                table: "RoomAllocationRequirements",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomAllocationRequirements_SeasonRanges_SeasonRangeId",
                table: "RoomAllocationRequirements",
                column: "SeasonRangeId",
                principalTable: "SeasonRanges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomAvailabilityRestrictions_Contracts_ContractId",
                table: "RoomAvailabilityRestrictions",
                column: "ContractId",
                principalTable: "Contracts",
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
                name: "FK_RoomPromotionalOffers_Contracts_ContractId",
                table: "RoomPromotionalOffers",
                column: "ContractId",
                principalTable: "Contracts",
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
                name: "FK_RoomRates_Seasons_SeasonId",
                table: "RoomRates",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Accommodations_AccommodationId",
                table: "Rooms",
                column: "AccommodationId",
                principalTable: "Accommodations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeasonRanges_Seasons_SeasonId",
                table: "SeasonRanges",
                column: "SeasonId",
                principalTable: "Seasons",
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
    }
}
