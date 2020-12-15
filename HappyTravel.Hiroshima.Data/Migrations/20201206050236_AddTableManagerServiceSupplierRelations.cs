using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class AddTableManagerServiceSupplierRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMaster",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "Permissions",
                table: "Managers");

            migrationBuilder.CreateTable(
                name: "ManagerServiceSupplierRelations",
                columns: table => new
                {
                    ManagerId = table.Column<int>(type: "integer", nullable: false),
                    ServiceSupplierId = table.Column<int>(type: "integer", nullable: false),
                    ManagerPermissions = table.Column<int>(type: "integer", nullable: false, defaultValue: 2147483647),
                    IsMaster = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerServiceSupplierRelations", x => new { x.ManagerId, x.ServiceSupplierId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManagerServiceSupplierRelations");

            migrationBuilder.AddColumn<bool>(
                name: "IsMaster",
                table: "Managers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Permissions",
                table: "Managers",
                type: "integer",
                nullable: false,
                defaultValue: 2147483646);
        }
    }
}
