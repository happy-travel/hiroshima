using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class ChangedTableImagesRenameColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Images",
                newName: "OriginalName");

            migrationBuilder.RenameColumn(
                name: "MimeType",
                table: "Images",
                newName: "OriginalContentType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OriginalName",
                table: "Images",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "OriginalContentType",
                table: "Images",
                newName: "MimeType");
        }
    }
}
