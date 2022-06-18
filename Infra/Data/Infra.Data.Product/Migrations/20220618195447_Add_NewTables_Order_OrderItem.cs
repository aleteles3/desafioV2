using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Product.Migrations
{
    public partial class Add_NewTables_Order_OrderItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Pro_Price",
                schema: "Product",
                table: "Pro_Product",
                newName: "Pro_ListPrice");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CascadeMode = table.Column<int>(type: "integer", nullable: false),
                    DateInc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateAlter = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ord_Order",
                schema: "Product",
                columns: table => new
                {
                    Ord_OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Ord_OrderStatus = table.Column<int>(type: "integer", nullable: false),
                    Use_UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateInc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateAlter = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ord_Order", x => x.Ord_OrderId);
                    table.ForeignKey(
                        name: "FK_Ord_Order_Use_User_Use_UserId",
                        column: x => x.Use_UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ori_OrderItem",
                schema: "Product",
                columns: table => new
                {
                    Ori_OrderItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Ori_ListPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Ori_Discount = table.Column<decimal>(type: "numeric", nullable: false),
                    Ord_OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Pro_ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateInc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateAlter = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ori_OrderItem", x => x.Ori_OrderItemId);
                    table.ForeignKey(
                        name: "FK_Ori_OderItem_Ord_Order_Ord_OrderId",
                        column: x => x.Ord_OrderId,
                        principalSchema: "Product",
                        principalTable: "Ord_Order",
                        principalColumn: "Ord_OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ori_OrderItem_Pro_Product_Pro_ProductId",
                        column: x => x.Pro_ProductId,
                        principalSchema: "Product",
                        principalTable: "Pro_Product",
                        principalColumn: "Pro_ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ord_Order_Use_UserId",
                schema: "Product",
                table: "Ord_Order",
                column: "Use_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ori_OrderItem_Ord_OrderId",
                schema: "Product",
                table: "Ori_OrderItem",
                column: "Ord_OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Ori_OrderItem_Pro_ProductId",
                schema: "Product",
                table: "Ori_OrderItem",
                column: "Pro_ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ori_OrderItem",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "Ord_Order",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.RenameColumn(
                name: "Pro_ListPrice",
                schema: "Product",
                table: "Pro_Product",
                newName: "Pro_Price");
        }
    }
}
