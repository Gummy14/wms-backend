using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringContainerTableToIncludeHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemContainerEventHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Containers",
                table: "Containers");

            migrationBuilder.RenameColumn(
                name: "DateTimeRegistered",
                table: "Containers",
                newName: "EventDateTime");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Containers",
                newName: "PreviousContainerEventId");

            migrationBuilder.AddColumn<Guid>(
                name: "ContainerEventId",
                table: "Containers",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "ContainerId",
                table: "Containers",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "EventType",
                table: "Containers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "NextContainerEventId",
                table: "Containers",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Containers",
                table: "Containers",
                column: "ContainerEventId");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "EventTypeDescription",
                value: "Registered");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "EventTypeDescription",
                value: "Putaway");

            migrationBuilder.InsertData(
                table: "EventTypes",
                columns: new[] { "Id", "EventTypeDescription" },
                values: new object[] { 3, "Picked" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Containers",
                table: "Containers");

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "ContainerEventId",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "ContainerId",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "EventType",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "NextContainerEventId",
                table: "Containers");

            migrationBuilder.RenameColumn(
                name: "PreviousContainerEventId",
                table: "Containers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "EventDateTime",
                table: "Containers",
                newName: "DateTimeRegistered");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Containers",
                table: "Containers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ItemContainerEventHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ContainerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DateTimeStamp = table.Column<DateTime>(type: "datetime", nullable: false),
                    EventType = table.Column<int>(type: "int", nullable: false)
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
    }
}
