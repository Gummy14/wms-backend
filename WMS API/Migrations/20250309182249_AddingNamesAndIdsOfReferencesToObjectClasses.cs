using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class AddingNamesAndIdsOfReferencesToObjectClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 521);

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "Locations",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContainerName",
                table: "Items",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LocationName",
                table: "Items",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrderName",
                table: "Items",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 522,
                column: "EventTypeDescription",
                value: "Container Selected For Picking");

            migrationBuilder.InsertData(
                table: "EventTypes",
                columns: new[] { "Id", "EventTypeDescription" },
                values: new object[] { 523, "Item Picked Into Container" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 523);

            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "ContainerName",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "LocationName",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "OrderName",
                table: "Items");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 522,
                column: "EventTypeDescription",
                value: "Item Picked Into Container");

            migrationBuilder.InsertData(
                table: "EventTypes",
                columns: new[] { "Id", "EventTypeDescription" },
                values: new object[] { 521, "Container Selected For Picking" });
        }
    }
}
