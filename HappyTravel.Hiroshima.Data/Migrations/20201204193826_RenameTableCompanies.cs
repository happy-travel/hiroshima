using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class RenameTableCompanies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Managers",
                newName: "ServiceSupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Managers_CompanyId",
                table: "Managers",
                newName: "IX_Managers_ServiceSupplierId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Images",
                newName: "ServiceSupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_CompanyId",
                table: "Images",
                newName: "IX_Images_ServiceSupplierId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Documents",
                newName: "ServiceSupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_CompanyId",
                table: "Documents",
                newName: "IX_Documents_ServiceSupplierId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Contracts",
                newName: "ServiceSupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_CompanyId",
                table: "Contracts",
                newName: "IX_Contracts_ServiceSupplierId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "BookingOrders",
                newName: "ServiceSupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingOrders_CompanyId",
                table: "BookingOrders",
                newName: "IX_BookingOrders_ServiceSupplierId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Accommodations",
                newName: "ServiceSupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Accommodations_CompanyId",
                table: "Accommodations",
                newName: "IX_Accommodations_ServiceSupplierId");

            migrationBuilder.CreateTable(
                name: "ServiceSuppliers",
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
                    table.PrimaryKey("PK_ServiceSuppliers", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_ServiceSuppliers_ServiceSupplierId",
                table: "Accommodations",
                column: "ServiceSupplierId",
                principalTable: "ServiceSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingOrders_ServiceSuppliers_ServiceSupplierId",
                table: "BookingOrders",
                column: "ServiceSupplierId",
                principalTable: "ServiceSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_ServiceSuppliers_ServiceSupplierId",
                table: "Contracts",
                column: "ServiceSupplierId",
                principalTable: "ServiceSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_ServiceSuppliers_ServiceSupplierId",
                table: "Documents",
                column: "ServiceSupplierId",
                principalTable: "ServiceSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_ServiceSuppliers_ServiceSupplierId",
                table: "Images",
                column: "ServiceSupplierId",
                principalTable: "ServiceSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_ServiceSuppliers_ServiceSupplierId",
                table: "Managers",
                column: "ServiceSupplierId",
                principalTable: "ServiceSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_ServiceSuppliers_ServiceSupplierId",
                table: "Accommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingOrders_ServiceSuppliers_ServiceSupplierId",
                table: "BookingOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_ServiceSuppliers_ServiceSupplierId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_ServiceSuppliers_ServiceSupplierId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_ServiceSuppliers_ServiceSupplierId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_ServiceSuppliers_ServiceSupplierId",
                table: "Managers");

            migrationBuilder.DropTable(
                name: "ServiceSuppliers");

            migrationBuilder.RenameColumn(
                name: "ServiceSupplierId",
                table: "Managers",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Managers_ServiceSupplierId",
                table: "Managers",
                newName: "IX_Managers_CompanyId");

            migrationBuilder.RenameColumn(
                name: "ServiceSupplierId",
                table: "Images",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_ServiceSupplierId",
                table: "Images",
                newName: "IX_Images_CompanyId");

            migrationBuilder.RenameColumn(
                name: "ServiceSupplierId",
                table: "Documents",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_ServiceSupplierId",
                table: "Documents",
                newName: "IX_Documents_CompanyId");

            migrationBuilder.RenameColumn(
                name: "ServiceSupplierId",
                table: "Contracts",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_ServiceSupplierId",
                table: "Contracts",
                newName: "IX_Contracts_CompanyId");

            migrationBuilder.RenameColumn(
                name: "ServiceSupplierId",
                table: "BookingOrders",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingOrders_ServiceSupplierId",
                table: "BookingOrders",
                newName: "IX_BookingOrders_CompanyId");

            migrationBuilder.RenameColumn(
                name: "ServiceSupplierId",
                table: "Accommodations",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Accommodations_ServiceSupplierId",
                table: "Accommodations",
                newName: "IX_Accommodations_CompanyId");

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    PostalCode = table.Column<string>(type: "text", nullable: false),
                    Website = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

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
    }
}
