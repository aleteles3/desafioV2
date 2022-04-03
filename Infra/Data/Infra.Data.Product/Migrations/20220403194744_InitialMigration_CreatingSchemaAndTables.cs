using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Product.Migrations
{
    public partial class InitialMigration_CreatingSchemaAndTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Product");

            migrationBuilder.CreateTable(
                name: "Cat_Category",
                schema: "Product",
                columns: table => new
                {
                    Cat_CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Cat_Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CascadeMode = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cat_Category", x => x.Cat_CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Pro_Product",
                schema: "Product",
                columns: table => new
                {
                    Pro_ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Pro_Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Pro_Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Pro_Price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Pro_Stock = table.Column<int>(type: "integer", nullable: false),
                    Cat_CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CascadeMode = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pro_Product", x => x.Pro_ProductId);
                    table.ForeignKey(
                        name: "FK_Pro_Product_Cat_Category_Cat_CategoryId",
                        column: x => x.Cat_CategoryId,
                        principalSchema: "Product",
                        principalTable: "Cat_Category",
                        principalColumn: "Cat_CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pro_Product_Cat_CategoryId",
                schema: "Product",
                table: "Pro_Product",
                column: "Cat_CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pro_Product",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "Cat_Category",
                schema: "Product");
        }
    }
}
