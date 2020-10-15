using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class ModifyContractManagerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ContractManagers");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ContractManagers",
                newName: "Position");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ContractManagers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "ContractManagers",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "ContractManagers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "ContractManagers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ContractManagers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "ContractManagers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "ContractManagers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "ContractManagers",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ContractManagers_Email",
                table: "ContractManagers");

            migrationBuilder.DropIndex(
                name: "IX_ContractManagers_IdentityHash",
                table: "ContractManagers");

            migrationBuilder.DropColumn(
                name: "Fax",
                table: "ContractManagers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "ContractManagers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ContractManagers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "ContractManagers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "ContractManagers");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "ContractManagers");

            migrationBuilder.RenameColumn(
                name: "Position",
                table: "ContractManagers",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ContractManagers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "ContractManagers",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ContractManagers",
                type: "text",
                nullable: true);
        }
    }
}
