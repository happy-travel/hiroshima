using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class AddTableCompanies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_Managers_ManagerId",
                table: "Accommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingOrders_Managers_ManagerId",
                table: "BookingOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Managers_ManagerId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Managers_ManagerId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Managers_ManagerId",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "Images",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_ManagerId",
                table: "Images",
                newName: "IX_Images_CompanyId");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "Documents",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_ManagerId",
                table: "Documents",
                newName: "IX_Documents_CompanyId");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "Contracts",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_ManagerId",
                table: "Contracts",
                newName: "IX_Contracts_CompanyId");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "BookingOrders",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingOrders_ManagerId",
                table: "BookingOrders",
                newName: "IX_BookingOrders_CompanyId");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "Accommodations",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Accommodations_ManagerId",
                table: "Accommodations",
                newName: "IX_Accommodations_CompanyId");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Managers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    PostalCode = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Website = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Managers_CompanyId",
                table: "Managers",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_Companies_CompanyId",
                table: "Accommodations",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingOrders_Companies_CompanyId",
                table: "BookingOrders",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Companies_CompanyId",
                table: "Contracts",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Companies_CompanyId",
                table: "Documents",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Companies_CompanyId",
                table: "Images",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Companies_CompanyId",
                table: "Managers",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_Companies_CompanyId",
                table: "Accommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingOrders_Companies_CompanyId",
                table: "BookingOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Companies_CompanyId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Companies_CompanyId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Companies_CompanyId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Companies_CompanyId",
                table: "Managers");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Managers_CompanyId",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Managers");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Images",
                newName: "ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_CompanyId",
                table: "Images",
                newName: "IX_Images_ManagerId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Documents",
                newName: "ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_CompanyId",
                table: "Documents",
                newName: "IX_Documents_ManagerId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Contracts",
                newName: "ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_CompanyId",
                table: "Contracts",
                newName: "IX_Contracts_ManagerId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "BookingOrders",
                newName: "ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingOrders_CompanyId",
                table: "BookingOrders",
                newName: "IX_BookingOrders_ManagerId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Accommodations",
                newName: "ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Accommodations_CompanyId",
                table: "Accommodations",
                newName: "IX_Accommodations_ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_Managers_ManagerId",
                table: "Accommodations",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingOrders_Managers_ManagerId",
                table: "BookingOrders",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Managers_ManagerId",
                table: "Contracts",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Managers_ManagerId",
                table: "Documents",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Managers_ManagerId",
                table: "Images",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
