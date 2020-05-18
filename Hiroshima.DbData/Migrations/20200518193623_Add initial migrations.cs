using System;
using System.Collections.Generic;
using Hiroshima.Common.Models;
using Hiroshima.DbData.Models.Accommodation;
using Hiroshima.DbData.Models.Booking;
using Hiroshima.DbData.Models.Rooms;
using Hiroshima.DbData.Models.Rooms.Occupancy;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Hiroshima.DbData.Migrations
{
    public partial class Addinitialmigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,")
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "BookingOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReferenceCode = table.Column<string>(nullable: false),
                    LanguageCode = table.Column<string>(nullable: false),
                    StatusCode = table.Column<int>(nullable: false),
                    BookingDate = table.Column<DateTime>(nullable: false),
                    CheckInDate = table.Column<DateTime>(nullable: false),
                    CheckOutDate = table.Column<DateTime>(nullable: false),
                    Nationality = table.Column<string>(nullable: false),
                    Residency = table.Column<string>(nullable: false),
                    Rooms = table.Column<List<RoomGuests>>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<MultiLanguage<string>>(type: "jsonb", nullable: false),
                    Type = table.Column<int>(nullable: false),
                    CountryCode = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accommodations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<MultiLanguage<string>>(type: "jsonb", nullable: false),
                    Address = table.Column<MultiLanguage<string>>(type: "jsonb", nullable: false),
                    TextualDescription = table.Column<List<TextualDescription>>(type: "jsonb", nullable: true),
                    Coordinates = table.Column<Point>(type: "geometry (point)", nullable: false),
                    Rating = table.Column<int>(nullable: false),
                    CheckInTime = table.Column<string>(nullable: true),
                    CheckOutTime = table.Column<string>(nullable: true),
                    Pictures = table.Column<List<Picture>>(type: "jsonb", nullable: true),
                    Contacts = table.Column<Contacts>(type: "jsonb", nullable: false),
                    PropertyType = table.Column<int>(nullable: false),
                    AccommodationAmenities = table.Column<List<MultiLanguage<string>>>(type: "jsonb", nullable: true),
                    RoomAmenities = table.Column<List<string>>(type: "jsonb", nullable: true),
                    AdditionalInfo = table.Column<Dictionary<string, MultiLanguage<string>>>(type: "jsonb", nullable: true),
                    LocationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accommodations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accommodations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccommodationId = table.Column<int>(nullable: false),
                    Name = table.Column<MultiLanguage<string>>(type: "jsonb", nullable: false),
                    Description = table.Column<MultiLanguage<string>>(type: "jsonb", nullable: true),
                    Amenities = table.Column<List<MultiLanguage<string>>>(type: "jsonb", nullable: true),
                    PermittedOccupancies = table.Column<PermittedOccupancies>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Accommodations_AccommodationId",
                        column: x => x.AccommodationId,
                        principalTable: "Accommodations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomAllocationRequirements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoomId = table.Column<int>(nullable: false),
                    StartsFromDate = table.Column<DateTime>(nullable: false),
                    EndsToDate = table.Column<DateTime>(nullable: false),
                    ReleasePeriod = table.Column<ReleasePeriod>(type: "jsonb", nullable: true),
                    MinimumStayNights = table.Column<int>(nullable: true),
                    Allotment = table.Column<int>(nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomAllocationRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomAllocationRequirements_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomAvailabilityRestrictions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoomId = table.Column<int>(nullable: false),
                    StartsFromDate = table.Column<DateTime>(type: "jsonb", nullable: false),
                    EndsToDate = table.Column<DateTime>(type: "jsonb", nullable: false),
                    Restrictions = table.Column<SaleRestrictions>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomAvailabilityRestrictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomAvailabilityRestrictions_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomPromotionalOffers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoomId = table.Column<int>(nullable: false),
                    BookByDate = table.Column<DateTime>(nullable: false),
                    ValidFromDate = table.Column<DateTime>(nullable: false),
                    ValidToDate = table.Column<DateTime>(nullable: false),
                    DiscountPercent = table.Column<double>(nullable: false),
                    BookingCode = table.Column<string>(nullable: false),
                    Details = table.Column<MultiLanguage<string>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomPromotionalOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomPromotionalOffers_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomRates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoomId = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    StartsFromDate = table.Column<DateTime>(nullable: false),
                    EndsToDate = table.Column<DateTime>(nullable: false),
                    CurrencyCode = table.Column<string>(nullable: false),
                    BoardBasis = table.Column<string>(nullable: true),
                    MealPlan = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomRates_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_LocationId",
                table: "Accommodations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomAllocationRequirements_RoomId",
                table: "RoomAllocationRequirements",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomAvailabilityRestrictions_RoomId",
                table: "RoomAvailabilityRestrictions",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomPromotionalOffers_RoomId",
                table: "RoomPromotionalOffers",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomRates_RoomId",
                table: "RoomRates",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_AccommodationId",
                table: "Rooms",
                column: "AccommodationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingOrders");

            migrationBuilder.DropTable(
                name: "RoomAllocationRequirements");

            migrationBuilder.DropTable(
                name: "RoomAvailabilityRestrictions");

            migrationBuilder.DropTable(
                name: "RoomPromotionalOffers");

            migrationBuilder.DropTable(
                name: "RoomRates");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Accommodations");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
