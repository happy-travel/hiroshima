using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class ChangeTableDocumentsRemamedColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Contracts_ContractId",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "MimeType",
                table: "Documents",
                newName: "ContentType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContentType",
                table: "Documents",
                newName: "MimeType");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Contracts_ContractId",
                table: "Documents",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
