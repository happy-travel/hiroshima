using System;
using System.Collections.Generic;
using Hiroshima.Common.Models;
using Hiroshima.Common.Models.Accommodation;
using Hiroshima.DbData.Models.Accommodation;
using Hiroshima.DbData.Models.Booking;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

namespace Hiroshima.DbData.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,")
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateSequence<int>(
                name: "AccommodationIdSeq");

            migrationBuilder.CreateSequence<int>(
                name: "BookingIdSeq");

            migrationBuilder.CreateSequence<int>(
                name: "CancelationPolicyIdSeq");

            migrationBuilder.CreateSequence<int>(
                name: "DiscountRateIdSeq");

            migrationBuilder.CreateSequence<int>(
                name: "LocalityIdSeq");

            migrationBuilder.CreateSequence<int>(
                name: "LocationIdSeq");

            migrationBuilder.CreateSequence<int>(
                name: "RateIdSeq");

            migrationBuilder.CreateSequence<int>(
                name: "RoomDetailsIdSeq");

            migrationBuilder.CreateSequence<int>(
                name: "RoomsIdSeq");

            migrationBuilder.CreateSequence<int>(
                name: "SeasonIdSeq");

            migrationBuilder.CreateSequence<int>(
                name: "StopSaleDateIdSeq");

            migrationBuilder.CreateTable(
                name: "Accommodations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"AccommodationIdSeq\"')"),
                    Name = table.Column<MultiLanguage<string>>(type: "jsonb", nullable: false),
                    Contacts = table.Column<Contacts>(type: "jsonb", nullable: true),
                    Picture = table.Column<Picture>(type: "jsonb", nullable: true),
                    Rating = table.Column<int>(nullable: false),
                    TextualDescription = table.Column<TextualDescription>(type: "jsonb", nullable: false),
                    Schedule = table.Column<Schedule>(type: "jsonb", nullable: true),
                    PropertyType = table.Column<int>(nullable: false),
                    Features = table.Column<List<FeatureInfo>>(type: "jsonb", nullable: true),
                    Amenities = table.Column<List<MultiLanguage<string>>>(type: "jsonb", nullable: true),
                    RoomAmenities = table.Column<List<MultiLanguage<string>>>(type: "jsonb", nullable: true),
                    AdditionalInfo = table.Column<Dictionary<string, MultiLanguage<string>>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accommodations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CancelationPolicies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CancelationPolicyDetails = table.Column<List<CancelationPolicyDetails>>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancelationPolicies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<MultiLanguage<string>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"RoomsIdSeq\"')"),
                    AccommodationId = table.Column<int>(nullable: false),
                    Name = table.Column<MultiLanguage<string>>(type: "jsonb", nullable: true),
                    Description = table.Column<MultiLanguage<string>>(type: "jsonb", nullable: true),
                    Amenities = table.Column<List<MultiLanguage<string>>>(type: "jsonb", nullable: true)
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
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"SeasonIdSeq\"')"),
                    Name = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    CancelationPolicyId = table.Column<int>(nullable: false),
                    AccommodationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seasons_Accommodations_AccommodationId",
                        column: x => x.AccommodationId,
                        principalTable: "Accommodations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Seasons_CancelationPolicies_CancelationPolicyId",
                        column: x => x.CancelationPolicyId,
                        principalTable: "CancelationPolicies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<MultiLanguage<string>>(type: "jsonb", nullable: false),
                    RegionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Countries_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscountRates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoomId = table.Column<int>(nullable: false),
                    DiscountPct = table.Column<int>(nullable: false),
                    BookingCode = table.Column<string>(nullable: true),
                    BookBy = table.Column<DateTime>(nullable: false),
                    ValidFrom = table.Column<DateTime>(nullable: false),
                    ValidTo = table.Column<DateTime>(nullable: false),
                    Details = table.Column<MultiLanguage<string>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountRates_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"RoomDetailsIdSeq\"')"),
                    RoomId = table.Column<int>(nullable: false),
                    AdultsNumber = table.Column<int>(nullable: false, defaultValue: 0),
                    InfantsNumber = table.Column<int>(nullable: false),
                    ChildrenNumber = table.Column<int>(nullable: false, defaultValue: 0),
                    ChildrenAges = table.Column<NpgsqlRange<int>>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomDetails_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StopSaleDates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"StopSaleDateIdSeq\"')"),
                    RoomId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StopSaleDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StopSaleDates_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractedRates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"RateIdSeq\"')"),
                    RoomId = table.Column<int>(nullable: false),
                    BoardBasisCode = table.Column<string>(nullable: true),
                    MealPlanCode = table.Column<string>(nullable: true),
                    CurrencyCode = table.Column<string>(nullable: true),
                    ReleaseDays = table.Column<int>(nullable: false),
                    MinimumStayDays = table.Column<int>(nullable: false),
                    SeasonPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    ExtraPersonPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    SeasonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractedRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractedRates_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractedRates_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Localities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"LocalityIdSeq\"')"),
                    Name = table.Column<MultiLanguage<string>>(type: "jsonb", nullable: false),
                    CountryCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Localities_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"BookingIdSeq\"')"),
                    StatusCode = table.Column<int>(nullable: false),
                    ContractRateId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "NOW()"),
                    BookedAt = table.Column<DateTime>(nullable: false),
                    CheckInAt = table.Column<DateTime>(nullable: false),
                    CheckOutAt = table.Column<DateTime>(nullable: false),
                    Nationality = table.Column<string>(nullable: true),
                    Residency = table.Column<string>(nullable: true),
                    LeadPassengerName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Booking_ContractedRates_ContractRateId",
                        column: x => x.ContractRateId,
                        principalTable: "ContractedRates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"LocationIdSeq\"')"),
                    Coordinates = table.Column<Point>(type: "geometry (point)", nullable: false),
                    Address = table.Column<MultiLanguage<string>>(type: "jsonb", nullable: false),
                    LocalityId = table.Column<int>(nullable: false),
                    AccommodationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_Accommodations_AccommodationId",
                        column: x => x.AccommodationId,
                        principalTable: "Accommodations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Locations_Localities_LocalityId",
                        column: x => x.LocalityId,
                        principalTable: "Localities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_ContractRateId",
                table: "Booking",
                column: "ContractRateId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractedRates_RoomId",
                table: "ContractedRates",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractedRates_SeasonId",
                table: "ContractedRates",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_RegionId",
                table: "Countries",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountRates_RoomId",
                table: "DiscountRates",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Localities_CountryCode",
                table: "Localities",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_AccommodationId",
                table: "Locations",
                column: "AccommodationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_LocalityId",
                table: "Locations",
                column: "LocalityId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomDetails_RoomId",
                table: "RoomDetails",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_AccommodationId",
                table: "Rooms",
                column: "AccommodationId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_AccommodationId",
                table: "Seasons",
                column: "AccommodationId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_CancelationPolicyId",
                table: "Seasons",
                column: "CancelationPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_StopSaleDates_RoomId",
                table: "StopSaleDates",
                column: "RoomId");

            migrationBuilder.Sql(@"CREATE INDEX accommodations_name_eng_fts_idx ON ""Accommodations"" USING GIN (to_tsvector('english', ""Name""));");
            migrationBuilder.Sql(@"CREATE INDEX accommodations_name_smpl_fts_idx ON ""Accommodations"" USING GIN (to_tsvector('simple', ""Name""));");
            migrationBuilder.Sql(@"CREATE INDEX locations_address_eng_fts_idx ON ""Locations"" USING GIN (to_tsvector('english', ""Address""));");
            migrationBuilder.Sql(@"CREATE INDEX locations_address_smpl_fts_idx ON ""Locations"" USING GIN (to_tsvector('simple', ""Address""));");
            InitData.AddStaticData(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "DiscountRates");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "RoomDetails");

            migrationBuilder.DropTable(
                name: "StopSaleDates");

            migrationBuilder.DropTable(
                name: "ContractedRates");

            migrationBuilder.DropTable(
                name: "Localities");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Accommodations");

            migrationBuilder.DropTable(
                name: "CancelationPolicies");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropSequence(
                name: "AccommodationIdSeq");

            migrationBuilder.DropSequence(
                name: "BookingIdSeq");

            migrationBuilder.DropSequence(
                name: "CancelationPolicyIdSeq");

            migrationBuilder.DropSequence(
                name: "DiscountRateIdSeq");

            migrationBuilder.DropSequence(
                name: "LocalityIdSeq");

            migrationBuilder.DropSequence(
                name: "LocationIdSeq");

            migrationBuilder.DropSequence(
                name: "RateIdSeq");

            migrationBuilder.DropSequence(
                name: "RoomDetailsIdSeq");

            migrationBuilder.DropSequence(
                name: "RoomsIdSeq");

            migrationBuilder.DropSequence(
                name: "SeasonIdSeq");

            migrationBuilder.DropSequence(
                name: "StopSaleDateIdSeq");

            migrationBuilder.Sql(@"DROP accommodations_name_eng_fts_idx;");
            migrationBuilder.Sql(@"DROP accommodations_name_smpl_fts_idx;");
            migrationBuilder.Sql(@"DROP locations_address_eng_fts_idx;");
            migrationBuilder.Sql(@"DROP locations_address_smpl_fts_idx;");
        }
    }
}
