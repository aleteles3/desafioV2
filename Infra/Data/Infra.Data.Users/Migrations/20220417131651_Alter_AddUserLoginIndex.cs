using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Users.Migrations
{
    public partial class Alter_AddUserLoginIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Use_Login",
                schema: "User",
                table: "Use_User",
                column: "Use_Login");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Use_Login",
                schema: "User",
                table: "Use_User");
        }
    }
}
