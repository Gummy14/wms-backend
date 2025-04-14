using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class RenamingStatusToEventType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "OrderData",
                newName: "EventType");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "LocationData",
                newName: "EventType");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ItemData",
                newName: "EventType");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ContainerData",
                newName: "EventType");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "BoxData",
                newName: "EventType");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 310,
                column: "EventTypeDescription",
                value: "Item Registered");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 410,
                column: "EventTypeDescription",
                value: "Item Putaway Into Location");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 521,
                column: "EventTypeDescription",
                value: "Container Added To Order");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EventType",
                table: "OrderData",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "EventType",
                table: "LocationData",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "EventType",
                table: "ItemData",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "EventType",
                table: "ContainerData",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "EventType",
                table: "BoxData",
                newName: "Status");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 310,
                column: "EventTypeDescription",
                value: "Item Newly Registered, Waiting To Be Selected For Putaway");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 410,
                column: "EventTypeDescription",
                value: "Item Putaway Into Location Complete");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 521,
                column: "EventTypeDescription",
                value: "Container Selected For Picking");
        }
    }
}
