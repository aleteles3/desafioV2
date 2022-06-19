using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Product.Migrations
{
    public partial class Alter_OrderItem_AddColumnQuantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Ori_Quantity",
                schema: "Product",
                table: "Ori_OrderItem",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ori_Quantity",
                schema: "Product",
                table: "Ori_OrderItem");
        }
    }
}
