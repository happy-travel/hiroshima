using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class ChangedTableImagesAddSecondKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Key",
                table: "Images",
                newName: "SmallImageKey");

            migrationBuilder.AddColumn<string>(
                name: "LargeImageKey",
                table: "Images",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Contracts_ContractId",
                table: "Documents",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Contracts_ContractId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "LargeImageKey",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "SmallImageKey",
                table: "Images",
                newName: "Key");
        }
    }
}
