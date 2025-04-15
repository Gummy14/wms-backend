using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class AddingBoxDataObjectToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "BoxData",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_BoxData_OrderId",
                table: "BoxData",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BoxData_Orders_OrderId",
                table: "BoxData",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoxData_Orders_OrderId",
                table: "BoxData");

            migrationBuilder.DropIndex(
                name: "IX_BoxData_OrderId",
                table: "BoxData");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "BoxData");
        }
    }
}
