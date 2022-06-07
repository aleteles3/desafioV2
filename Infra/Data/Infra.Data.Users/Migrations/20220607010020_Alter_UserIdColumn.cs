using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Users.Migrations
{
    public partial class Alter_UserIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "User",
                table: "Use_User",
                newName: "Use_UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Use_UserId",
                schema: "User",
                table: "Use_User",
                newName: "Id");
        }
    }
}
