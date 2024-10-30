using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class Refactoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Containers_ContainerId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ContainerId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ContainerId",
                table: "Items");

            migrationBuilder.AddColumn<Guid>(
                name: "ItemId",
                table: "Containers",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Containers_ItemId",
                table: "Containers",
                column: "ItemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Containers_Items_ItemId",
                table: "Containers",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Containers_Items_ItemId",
                table: "Containers");

            migrationBuilder.DropIndex(
                name: "IX_Containers_ItemId",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Containers");

            migrationBuilder.AddColumn<Guid>(
                name: "ContainerId",
                table: "Items",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ContainerId",
                table: "Items",
                column: "ContainerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Containers_ContainerId",
                table: "Items",
                column: "ContainerId",
                principalTable: "Containers",
                principalColumn: "Id");
        }
    }
}
