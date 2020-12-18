using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class AddTableManagerInvitations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ManagerInvitations",
                columns: table => new
                {
                    CodeHash = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    ManagerId = table.Column<int>(type: "integer", nullable: false),
                    ServiceSupplierId = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsAccepted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsResent = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerInvitations", x => x.CodeHash);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManagerInvitations");
        }
    }
}
