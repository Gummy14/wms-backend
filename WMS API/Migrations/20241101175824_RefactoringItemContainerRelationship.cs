using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringItemContainerRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "ContainerEventId",
                table: "Items",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "ItemEventId",
                table: "Containers",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "EventTypeDescription",
                value: "Added To Order");

            migrationBuilder.InsertData(
                table: "EventTypes",
                columns: new[] { "Id", "EventTypeDescription" },
                values: new object[] { 4, "Picked" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "ContainerEventId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemEventId",
                table: "Containers");

            migrationBuilder.AddColumn<Guid>(
                name: "ItemId",
                table: "Containers",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "EventTypeDescription",
                value: "Picked");

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
                principalColumn: "ItemEventId");
        }
    }
}
