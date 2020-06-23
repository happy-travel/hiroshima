using System;
using System.Collections.Generic;
using System.Text.Json;
using Hiroshima.Common.Models;
using Hiroshima.Common.Models.Enums;
using Hiroshima.DbData.Models.Accommodation;
using Hiroshima.DbData.Models.Booking;
using Hiroshima.DbData.Models.Location;
using Hiroshima.DbData.Models.Rooms;
using Hiroshima.DbData.Models.Rooms.CancellationPolicies;
using Hiroshima.DbData.Models.Rooms.Occupancy;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Hiroshima.DbData.Migrations
{
    public partial class AddInitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:accommodation_rating", "unknown,not_rated,one_star,two_stars,three_stars,four_stars,five_stars")
                .Annotation("Npgsql:Enum:location_types", "zone,city,country,point_of_interest")
                .Annotation("Npgsql:Enum:sale_restrictions", "stop_sale")
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
                name: "CancellationPolicies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoomId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    CancellationPolicyData = table.Column<List<CancellationPolicyData>>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancellationPolicies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<JsonDocument>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    Type = table.Column<LocationTypes>(nullable: false),
                    CountryCode = table.Column<string>(nullable: false),
                    ParentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accommodations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    Address = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    TextualDescription = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    Coordinates = table.Column<Point>(type: "geometry (point)", nullable: false),
                    Rating = table.Column<AccommodationRating>(nullable: false),
                    CheckInTime = table.Column<string>(nullable: true),
                    CheckOutTime = table.Column<string>(nullable: true),
                    Pictures = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    Contacts = table.Column<Contacts>(type: "jsonb", nullable: false),
                    PropertyType = table.Column<int>(nullable: false),
                    AccommodationAmenities = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    AdditionalInfo = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    OccupancyDefinition = table.Column<OccupancyDefinition>(type: "jsonb", nullable: true),
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
                    Name = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    Description = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    Amenities = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    OccupancyConfigurations = table.Column<List<OccupancyConfiguration>>(type: "jsonb", nullable: false)
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
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    ReleasePeriod = table.Column<ReleasePeriod>(type: "jsonb", nullable: true),
                    MinimumStayNights = table.Column<int>(nullable: true),
                    Allotment = table.Column<int>(nullable: true)
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
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Restrictions = table.Column<SaleRestrictions>(nullable: false, defaultValue: SaleRestrictions.StopSale)
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
                    Details = table.Column<JsonDocument>(type: "jsonb", nullable: true)
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
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    CurrencyCode = table.Column<string>(nullable: false),
                    BoardBasis = table.Column<string>(nullable: true),
                    MealPlan = table.Column<string>(nullable: true),
                    Details = table.Column<JsonDocument>(nullable: true)
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
                name: "IX_Accommodations_Coordinates",
                table: "Accommodations",
                column: "Coordinates")
                .Annotation("Npgsql:IndexMethod", "GIST");

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_LocationId",
                table: "Accommodations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_CountryCode",
                table: "Locations",
                column: "CountryCode");

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
            
            var langFromJsonbFunctionSql = @"CREATE FUNCTION lang_from_jsonb(multilangjson jsonb, languagecode text) returns jsonb
                                            IMMUTABLE
                                            LANGUAGE plpgsql
                                            AS
                                            $$
                                            DECLARE result jsonb;
                                                available_languages text[] := '{""ar"", ""bg"", ""de"", ""el"", ""en"", ""es"", ""fr"", ""it"", ""hu"", ""pl"", ""pt"", ""ro"", ""ru"", ""sr"", ""tr""}';
                                                lowerLanguage text := lower(languageCode);
                                            BEGIN
                                                IF NOT lowerLanguage = ANY(available_languages) THEN
                                                    RAISE 'Unknown language code: %', languageCode;
                                                END IF;
        
                                                SELECT jsonb_build_object(key, value) INTO result
                                                FROM jsonb_each(multiLangJson)
                                                WHERE key = lowerLanguage;
        
                                                IF result IS NULL THEN
                                                    SELECT jsonb_build_object(key, value) INTO result
                                                    FROM jsonb_each(multiLangJson)
                                                    WHERE key = 'en';
                                                END IF;
        
                                            RETURN result;
                                            END
                                            $$;";
            migrationBuilder.Sql(langFromJsonbFunctionSql);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingOrders");

            migrationBuilder.DropTable(
                name: "CancellationPolicies");

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

            migrationBuilder.DropTable(
                name: "Countries");
            
            migrationBuilder.Sql("DROP FUNCTION lang_from_jsonb(multilangjson jsonb, languagecode text)");
        }
    }
}
