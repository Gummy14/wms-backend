using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class RemovingLocationDataItemIdWillNowCheckLocationItemForFilledOrNow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationData_Items_ItemId",
                table: "LocationData");

            migrationBuilder.DropIndex(
                name: "IX_LocationData_ItemId",
                table: "LocationData");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "LocationData");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ItemId",
                table: "LocationData",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_LocationData_ItemId",
                table: "LocationData",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationData_Items_ItemId",
                table: "LocationData",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }
    }
}
