using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class ContractManagerFKs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_ContractManagers_ContractManagerId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_ContractManagers_ContractManagerId",
                table: "Images");
        }
    }
}
