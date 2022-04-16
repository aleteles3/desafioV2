using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Users.Migrations
{
    public partial class Alter_ChangingStatusField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "User",
                table: "Use_User",
                newName: "Use_Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Use_Status",
                schema: "User",
                table: "Use_User",
                newName: "Status");
        }
    }
}
