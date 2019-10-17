using System;
using System.Collections.Generic;
using Hiroshima.Common.Models;
using Hiroshima.DbData.Models.Accommodation;
using Hiroshima.DbData.Models.Common;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
                name: "LocalityIdSeq");

            migrationBuilder.CreateSequence<int>(
                name: "LocationIdSeq");

            migrationBuilder.CreateSequence<int>(
                name: "PermittedOccupancyIdSeq");

            migrationBuilder.CreateSequence<int>(
                name: "RateIdSeq");

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
                    Description = table.Column<MultiLanguage<string>>(type: "jsonb", nullable: false),
                    Contacts = table.Column<Contacts>(type: "jsonb", nullable: true),
                    Pictures = table.Column<List<Picture>>(type: "jsonb", nullable: true),
                    Rating = table.Column<int>(nullable: false),
                    TextualDescriptions = table.Column<List<TextualDescription>>(type: "jsonb", nullable: true),
                    Schedule = table.Column<Schedule>(type: "jsonb", nullable: true),
                    Category = table.Column<string>(nullable: true),
                    PropertyType = table.Column<int>(nullable: false),
                    Amenities = table.Column<List<MultiLanguage<string>>>(type: "jsonb", nullable: true),
                    AdditionalInfo = table.Column<Dictionary<string, MultiLanguage<string>>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accommodations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BoardBasis",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<MultiLanguage<string>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardBasis", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "BookingStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Code);
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
                    Amenities = table.Column<Dictionary<string, MultiLanguage<string>>>(type: "jsonb", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<MultiLanguage<string>>(type: "jsonb", nullable: true),
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
                name: "PermittedOccupancies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"PermittedOccupancyIdSeq\"')"),
                    RoomId = table.Column<int>(nullable: false),
                    AdultsNumber = table.Column<int>(nullable: false, defaultValue: 0),
                    ChildrenNumber = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermittedOccupancies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermittedOccupancies_Rooms_RoomId",
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
                name: "Rates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"RateIdSeq\"')"),
                    RoomId = table.Column<int>(nullable: false),
                    BoardBasisCode = table.Column<string>(nullable: true),
                    ReleaseDays = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    SeasonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rates_BoardBasis_BoardBasisCode",
                        column: x => x.BoardBasisCode,
                        principalTable: "BoardBasis",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rates_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rates_Seasons_SeasonId",
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
                    StatusId = table.Column<int>(nullable: false),
                    RateId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "NOW()"),
                    BookingAt = table.Column<DateTime>(nullable: false),
                    CheckInAt = table.Column<DateTime>(nullable: false),
                    CheckOutAt = table.Column<DateTime>(nullable: false),
                    Nationality = table.Column<string>(nullable: true),
                    Residency = table.Column<string>(nullable: true),
                    MainPassengerName = table.Column<MultiLanguage<string>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Booking_Rates_RateId",
                        column: x => x.RateId,
                        principalTable: "Rates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Booking_BookingStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "BookingStatus",
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
                name: "IX_Booking_RateId",
                table: "Booking",
                column: "RateId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_StatusId",
                table: "Booking",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_RegionId",
                table: "Countries",
                column: "RegionId");

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
                name: "IX_PermittedOccupancies_RoomId",
                table: "PermittedOccupancies",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_BoardBasisCode",
                table: "Rates",
                column: "BoardBasisCode");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_RoomId",
                table: "Rates",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_SeasonId",
                table: "Rates",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_AccommodationId",
                table: "Rooms",
                column: "AccommodationId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_AccommodationId",
                table: "Seasons",
                column: "AccommodationId");

            migrationBuilder.CreateIndex(
                name: "IX_StopSaleDates_RoomId",
                table: "StopSaleDates",
                column: "RoomId");

            InitData.AddStaticData(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "PermittedOccupancies");

            migrationBuilder.DropTable(
                name: "StopSaleDates");

            migrationBuilder.DropTable(
                name: "Rates");

            migrationBuilder.DropTable(
                name: "BookingStatus");

            migrationBuilder.DropTable(
                name: "Localities");

            migrationBuilder.DropTable(
                name: "BoardBasis");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Accommodations");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropSequence(
                name: "AccommodationIdSeq");

            migrationBuilder.DropSequence(
                name: "BookingIdSeq");

            migrationBuilder.DropSequence(
                name: "LocalityIdSeq");

            migrationBuilder.DropSequence(
                name: "LocationIdSeq");

            migrationBuilder.DropSequence(
                name: "PermittedOccupancyIdSeq");

            migrationBuilder.DropSequence(
                name: "RateIdSeq");

            migrationBuilder.DropSequence(
                name: "RoomsIdSeq");

            migrationBuilder.DropSequence(
                name: "SeasonIdSeq");

            migrationBuilder.DropSequence(
                name: "StopSaleDateIdSeq");
        }
    }
}
