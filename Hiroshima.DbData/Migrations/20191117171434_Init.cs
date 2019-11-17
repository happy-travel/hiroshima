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
                "AccommodationIdSeq");

            migrationBuilder.CreateSequence<int>(
                "BookingIdSeq");

            migrationBuilder.CreateSequence<int>(
                "CancelationPolicyIdSeq");

            migrationBuilder.CreateSequence<int>(
                "DiscountRateIdSeq");

            migrationBuilder.CreateSequence<int>(
                "LocalityIdSeq");

            migrationBuilder.CreateSequence<int>(
                "LocationIdSeq");

            migrationBuilder.CreateSequence<int>(
                "RateIdSeq");

            migrationBuilder.CreateSequence<int>(
                "RoomDetailsIdSeq");

            migrationBuilder.CreateSequence<int>(
                "RoomsIdSeq");

            migrationBuilder.CreateSequence<int>(
                "SeasonIdSeq");

            migrationBuilder.CreateSequence<int>(
                "StopSaleDateIdSeq");

            migrationBuilder.CreateTable(
                "Accommodations",
                table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"AccommodationIdSeq\"')"),
                    Name = table.Column<MultiLanguage<string>>("jsonb"),
                    Contacts = table.Column<Contacts>("jsonb", nullable: true),
                    Picture = table.Column<Picture>("jsonb", nullable: true),
                    Rating = table.Column<int>(),
                    TextualDescription = table.Column<TextualDescription>("jsonb"),
                    Schedule = table.Column<Schedule>("jsonb", nullable: true),
                    PropertyType = table.Column<int>(),
                    Features = table.Column<List<FeatureInfo>>("jsonb", nullable: true),
                    Amenities = table.Column<List<MultiLanguage<string>>>("jsonb", nullable: true),
                    RoomAmenities = table.Column<List<MultiLanguage<string>>>("jsonb", nullable: true),
                    AdditionalInfo = table.Column<Dictionary<string, MultiLanguage<string>>>("jsonb", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Accommodations", x => x.Id); });

            migrationBuilder.CreateTable(
                "CancelationPolicies",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CancelationPolicyDetails = table.Column<List<CancelationPolicyDetails>>("jsonb")
                },
                constraints: table => { table.PrimaryKey("PK_CancelationPolicies", x => x.Id); });

            migrationBuilder.CreateTable(
                "Regions",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<MultiLanguage<string>>("jsonb", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Regions", x => x.Id); });

            migrationBuilder.CreateTable(
                "Rooms",
                table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"RoomsIdSeq\"')"),
                    AccommodationId = table.Column<int>(),
                    Name = table.Column<MultiLanguage<string>>("jsonb", nullable: true),
                    Description = table.Column<MultiLanguage<string>>("jsonb", nullable: true),
                    Amenities = table.Column<List<MultiLanguage<string>>>("jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        "FK_Rooms_Accommodations_AccommodationId",
                        x => x.AccommodationId,
                        "Accommodations",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Seasons",
                table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"SeasonIdSeq\"')"),
                    Name = table.Column<string>(),
                    StartDate = table.Column<DateTime>(),
                    EndDate = table.Column<DateTime>(),
                    CancelationPolicyId = table.Column<int>(),
                    AccommodationId = table.Column<int>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                    table.ForeignKey(
                        "FK_Seasons_Accommodations_AccommodationId",
                        x => x.AccommodationId,
                        "Accommodations",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Seasons_CancelationPolicies_CancelationPolicyId",
                        x => x.CancelationPolicyId,
                        "CancelationPolicies",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Countries",
                table => new
                {
                    Code = table.Column<string>(),
                    Name = table.Column<MultiLanguage<string>>("jsonb"),
                    RegionId = table.Column<int>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Code);
                    table.ForeignKey(
                        "FK_Countries_Regions_RegionId",
                        x => x.RegionId,
                        "Regions",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "DiscountRates",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoomId = table.Column<int>(),
                    DiscountPct = table.Column<int>(),
                    BookingCode = table.Column<string>(nullable: true),
                    BookBy = table.Column<DateTime>(),
                    ValidFrom = table.Column<DateTime>(),
                    ValidTo = table.Column<DateTime>(),
                    Details = table.Column<MultiLanguage<string>>("jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountRates", x => x.Id);
                    table.ForeignKey(
                        "FK_DiscountRates_Rooms_RoomId",
                        x => x.RoomId,
                        "Rooms",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "RoomDetails",
                table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"RoomDetailsIdSeq\"')"),
                    RoomId = table.Column<int>(),
                    AdultsNumber = table.Column<int>(nullable: false, defaultValue: 0),
                    InfantsNumber = table.Column<int>(),
                    ChildrenNumber = table.Column<int>(nullable: false, defaultValue: 0),
                    ChildrenAges = table.Column<NpgsqlRange<int>>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomDetails", x => x.Id);
                    table.ForeignKey(
                        "FK_RoomDetails_Rooms_RoomId",
                        x => x.RoomId,
                        "Rooms",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "StopSaleDates",
                table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"StopSaleDateIdSeq\"')"),
                    RoomId = table.Column<int>(),
                    StartDate = table.Column<DateTime>(),
                    EndDate = table.Column<DateTime>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StopSaleDates", x => x.Id);
                    table.ForeignKey(
                        "FK_StopSaleDates_Rooms_RoomId",
                        x => x.RoomId,
                        "Rooms",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "ContractedRates",
                table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"RateIdSeq\"')"),
                    RoomId = table.Column<int>(),
                    BoardBasisCode = table.Column<string>(nullable: true),
                    MealPlanCode = table.Column<string>(nullable: true),
                    CurrencyCode = table.Column<string>(nullable: true),
                    ReleaseDays = table.Column<int>(),
                    MinimumStayDays = table.Column<int>(),
                    SeasonPrice = table.Column<decimal>("numeric"),
                    ExtraPersonPrice = table.Column<decimal>("numeric"),
                    SeasonId = table.Column<int>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractedRates", x => x.Id);
                    table.ForeignKey(
                        "FK_ContractedRates_Rooms_RoomId",
                        x => x.RoomId,
                        "Rooms",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_ContractedRates_Seasons_SeasonId",
                        x => x.SeasonId,
                        "Seasons",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Localities",
                table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"LocalityIdSeq\"')"),
                    Name = table.Column<MultiLanguage<string>>("jsonb"),
                    CountryCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localities", x => x.Id);
                    table.ForeignKey(
                        "FK_Localities_Countries_CountryCode",
                        x => x.CountryCode,
                        "Countries",
                        "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Booking",
                table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"BookingIdSeq\"')"),
                    StatusCode = table.Column<int>(),
                    ContractRateId = table.Column<int>(),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "NOW()"),
                    BookedAt = table.Column<DateTime>(),
                    CheckInAt = table.Column<DateTime>(),
                    CheckOutAt = table.Column<DateTime>(),
                    Nationality = table.Column<string>(nullable: true),
                    Residency = table.Column<string>(nullable: true),
                    LeadPassengerName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.Id);
                    table.ForeignKey(
                        "FK_Booking_ContractedRates_ContractRateId",
                        x => x.ContractRateId,
                        "ContractedRates",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Locations",
                table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"LocationIdSeq\"')"),
                    Coordinates = table.Column<Point>("geometry (point)"),
                    Address = table.Column<MultiLanguage<string>>("jsonb"),
                    LocalityId = table.Column<int>(),
                    AccommodationId = table.Column<int>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        "FK_Locations_Accommodations_AccommodationId",
                        x => x.AccommodationId,
                        "Accommodations",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Locations_Localities_LocalityId",
                        x => x.LocalityId,
                        "Localities",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_Booking_ContractRateId",
                "Booking",
                "ContractRateId");

            migrationBuilder.CreateIndex(
                "IX_ContractedRates_RoomId",
                "ContractedRates",
                "RoomId");

            migrationBuilder.CreateIndex(
                "IX_ContractedRates_SeasonId",
                "ContractedRates",
                "SeasonId");

            migrationBuilder.CreateIndex(
                "IX_Countries_RegionId",
                "Countries",
                "RegionId");

            migrationBuilder.CreateIndex(
                "IX_DiscountRates_RoomId",
                "DiscountRates",
                "RoomId");

            migrationBuilder.CreateIndex(
                "IX_Localities_CountryCode",
                "Localities",
                "CountryCode");

            migrationBuilder.CreateIndex(
                "IX_Locations_AccommodationId",
                "Locations",
                "AccommodationId",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Locations_LocalityId",
                "Locations",
                "LocalityId");

            migrationBuilder.CreateIndex(
                "IX_RoomDetails_RoomId",
                "RoomDetails",
                "RoomId");

            migrationBuilder.CreateIndex(
                "IX_Rooms_AccommodationId",
                "Rooms",
                "AccommodationId");

            migrationBuilder.CreateIndex(
                "IX_Seasons_AccommodationId",
                "Seasons",
                "AccommodationId");

            migrationBuilder.CreateIndex(
                "IX_Seasons_CancelationPolicyId",
                "Seasons",
                "CancelationPolicyId");

            migrationBuilder.CreateIndex(
                "IX_StopSaleDates_RoomId",
                "StopSaleDates",
                "RoomId");
            migrationBuilder.Sql(@"CREATE INDEX accommodations_name_eng_fts_idx ON ""Accommodations"" USING GIN (to_tsvector('english', ""Name""));");
            migrationBuilder.Sql(@"CREATE INDEX accommodations_name_smpl_fts_idx ON ""Accommodations"" USING GIN (to_tsvector('simple', ""Name""));");
            migrationBuilder.Sql(@"CREATE INDEX locations_address_eng_fts_idx ON ""Locations"" USING GIN (to_tsvector('english', ""Address""));");
            migrationBuilder.Sql(@"CREATE INDEX locations_address_smpl_fts_idx ON ""Locations"" USING GIN (to_tsvector('simple', ""Address""));");
            InitData.AddStaticData(migrationBuilder);
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Booking");

            migrationBuilder.DropTable(
                "DiscountRates");

            migrationBuilder.DropTable(
                "Locations");

            migrationBuilder.DropTable(
                "RoomDetails");

            migrationBuilder.DropTable(
                "StopSaleDates");

            migrationBuilder.DropTable(
                "ContractedRates");

            migrationBuilder.DropTable(
                "Localities");

            migrationBuilder.DropTable(
                "Rooms");

            migrationBuilder.DropTable(
                "Seasons");

            migrationBuilder.DropTable(
                "Countries");

            migrationBuilder.DropTable(
                "Accommodations");

            migrationBuilder.DropTable(
                "CancelationPolicies");

            migrationBuilder.DropTable(
                "Regions");

            migrationBuilder.DropSequence(
                "AccommodationIdSeq");

            migrationBuilder.DropSequence(
                "BookingIdSeq");

            migrationBuilder.DropSequence(
                "CancelationPolicyIdSeq");

            migrationBuilder.DropSequence(
                "DiscountRateIdSeq");

            migrationBuilder.DropSequence(
                "LocalityIdSeq");

            migrationBuilder.DropSequence(
                "LocationIdSeq");

            migrationBuilder.DropSequence(
                "RateIdSeq");

            migrationBuilder.DropSequence(
                "RoomDetailsIdSeq");

            migrationBuilder.DropSequence(
                "RoomsIdSeq");

            migrationBuilder.DropSequence(
                "SeasonIdSeq");

            migrationBuilder.DropSequence(
                "StopSaleDateIdSeq");
            migrationBuilder.Sql(@"DROP accommodations_name_eng_fts_idx;");
            migrationBuilder.Sql(@"DROP accommodations_name_smpl_fts_idx;");
            migrationBuilder.Sql(@"DROP locations_address_eng_fts_idx;");
            migrationBuilder.Sql(@"DROP locations_address_smpl_fts_idx;");
        }
    }
}