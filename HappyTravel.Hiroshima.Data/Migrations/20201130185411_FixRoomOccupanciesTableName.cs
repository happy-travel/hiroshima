using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class FixRoomOccupanciesTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomOccupations");

            migrationBuilder.CreateTable(
                name: "RoomOccupancies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FromDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ToDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    RoomId = table.Column<int>(type: "integer", nullable: false),
                    BookingOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomOccupancies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomOccupancies_BookingOrders_BookingOrderId",
                        column: x => x.BookingOrderId,
                        principalTable: "BookingOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_RoomOccupancies_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomOccupancies_BookingOrderId",
                table: "RoomOccupancies",
                column: "BookingOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomOccupancies_RoomId",
                table: "RoomOccupancies",
                column: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomOccupancies");

            migrationBuilder.CreateTable(
                name: "RoomOccupations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BookingOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    FromDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    RoomId = table.Column<int>(type: "integer", nullable: false),
                    ToDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomOccupations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomOccupations_BookingOrders_BookingOrderId",
                        column: x => x.BookingOrderId,
                        principalTable: "BookingOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_RoomOccupations_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomOccupations_BookingOrderId",
                table: "RoomOccupations",
                column: "BookingOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomOccupations_RoomId",
                table: "RoomOccupations",
                column: "RoomId");
        }
    }
}
