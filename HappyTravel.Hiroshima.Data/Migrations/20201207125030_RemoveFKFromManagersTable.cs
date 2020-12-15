using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class RemoveFKFromManagersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Managers_ServiceSuppliers_ServiceSupplierId",
                table: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_Managers_ServiceSupplierId",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "ServiceSupplierId",
                table: "Managers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceSupplierId",
                table: "Managers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Managers_ServiceSupplierId",
                table: "Managers",
                column: "ServiceSupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_ServiceSuppliers_ServiceSupplierId",
                table: "Managers",
                column: "ServiceSupplierId",
                principalTable: "ServiceSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
