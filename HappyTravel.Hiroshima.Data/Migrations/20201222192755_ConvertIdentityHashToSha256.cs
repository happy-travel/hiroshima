using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class ConvertIdentityHashToSha256 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = "UPDATE \"Managers\" SET \"IdentityHash\" = encode(sha256(\"IdentityHash\"::bytea), 'hex')";
            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        { }
    }
}
