using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class RenameTableContractManagers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_ContractManagers_ContractManagerId",
                table: "Accommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingOrders_ContractManagers_ContractManagerId",
                table: "BookingOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_ContractManagers_ContractManagerId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_ContractManagers_ContractManagerId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_ContractManagers_ContractManagerId",
                table: "Images");

            migrationBuilder.DropTable(
                name: "ContractManagers");

            migrationBuilder.RenameColumn(
                name: "ContractManagerId",
                table: "Images",
                newName: "ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_ContractManagerId",
                table: "Images",
                newName: "IX_Images_ManagerId");

            migrationBuilder.RenameColumn(
                name: "ContractManagerId",
                table: "Documents",
                newName: "ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_ContractManagerId",
                table: "Documents",
                newName: "IX_Documents_ManagerId");

            migrationBuilder.RenameColumn(
                name: "ContractManagerId",
                table: "Contracts",
                newName: "ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_ContractManagerId",
                table: "Contracts",
                newName: "IX_Contracts_ManagerId");

            migrationBuilder.RenameColumn(
                name: "ContractManagerId",
                table: "BookingOrders",
                newName: "ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingOrders_ContractManagerId",
                table: "BookingOrders",
                newName: "IX_BookingOrders_ManagerId");

            migrationBuilder.RenameColumn(
                name: "ContractManagerId",
                table: "Accommodations",
                newName: "ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Accommodations_ContractManagerId",
                table: "Accommodations",
                newName: "IX_Accommodations_ManagerId");

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdentityHash = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Fax = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    Updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Managers_Email",
                table: "Managers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Managers_IdentityHash",
                table: "Managers",
                column: "IdentityHash",
                unique: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "Images",
                newName: "ContractManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_ManagerId",
                table: "Images",
                newName: "IX_Images_ContractManagerId");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "Documents",
                newName: "ContractManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_ManagerId",
                table: "Documents",
                newName: "IX_Documents_ContractManagerId");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "Contracts",
                newName: "ContractManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_ManagerId",
                table: "Contracts",
                newName: "IX_Contracts_ContractManagerId");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "BookingOrders",
                newName: "ContractManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingOrders_ManagerId",
                table: "BookingOrders",
                newName: "IX_BookingOrders_ContractManagerId");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "Accommodations",
                newName: "ContractManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Accommodations_ManagerId",
                table: "Accommodations",
                newName: "IX_Accommodations_ContractManagerId");

            migrationBuilder.CreateTable(
                name: "ContractManagers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Fax = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    IdentityHash = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractManagers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractManagers_Email",
                table: "ContractManagers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractManagers_IdentityHash",
                table: "ContractManagers",
                column: "IdentityHash",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_ContractManagers_ContractManagerId",
                table: "Accommodations",
                column: "ContractManagerId",
                principalTable: "ContractManagers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingOrders_ContractManagers_ContractManagerId",
                table: "BookingOrders",
                column: "ContractManagerId",
                principalTable: "ContractManagers",
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
                name: "FK_Documents_ContractManagers_ContractManagerId",
                table: "Documents",
                column: "ContractManagerId",
                principalTable: "ContractManagers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_ContractManagers_ContractManagerId",
                table: "Images",
                column: "ContractManagerId",
                principalTable: "ContractManagers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
