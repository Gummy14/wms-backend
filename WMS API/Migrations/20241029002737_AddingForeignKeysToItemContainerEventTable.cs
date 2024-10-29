using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class AddingForeignKeysToItemContainerEventTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Statuses",
                table: "EventTypes");

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventTypes",
                table: "EventTypes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ItemContainerEventHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ContainerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    DateTimeStamp = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemContainerEventHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemContainerEventHistory_Containers_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "Containers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemContainerEventHistory_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "EventTypeDescription",
                value: "Putaway");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "EventTypeDescription",
                value: "Pick");

            migrationBuilder.CreateIndex(
                name: "IX_ItemContainerEventHistory_ContainerId",
                table: "ItemContainerEventHistory",
                column: "ContainerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemContainerEventHistory_ItemId",
                table: "ItemContainerEventHistory",
                column: "ItemId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemContainerEventHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventTypes",
                table: "EventTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Statuses",
                table: "EventTypes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "EventHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ChildId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DateTimeStamp = table.Column<DateTime>(type: "datetime", nullable: false),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventHistory", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "EventTypeDescription",
                value: "Item Registration");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "EventTypeDescription",
                value: "Container Registration");

            migrationBuilder.InsertData(
                table: "EventTypes",
                columns: new[] { "Id", "EventTypeDescription" },
                values: new object[,]
                {
                    { 3, "Row Registration" },
                    { 4, "Shelf Registration" },
                    { 5, "Putaway" },
                    { 6, "Pick" }
                });
        }
    }
}
