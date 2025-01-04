using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingEventTypesAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 220);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 411);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 420);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 421);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 422);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 423);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 610);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 620);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 710);

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 111,
                column: "EventTypeDescription",
                value: "Location Occupied");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 112,
                column: "EventTypeDescription",
                value: "Location Unoccupied");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 210,
                column: "EventTypeDescription",
                value: "Container Newly Registered, Not In Use");

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
                keyValue: 510,
                column: "EventTypeDescription",
                value: "Order Newly Registered, Waiting To Be Selected For Picking");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 520,
                column: "EventTypeDescription",
                value: "Order Selected For Picking, Picking In Progress");

            migrationBuilder.InsertData(
                table: "EventTypes",
                columns: new[] { "Id", "EventTypeDescription" },
                values: new object[,]
                {
                    { 110, "Location Newly Registered, Unoccupied" },
                    { 211, "Container In Use" },
                    { 212, "Container Not In Use" },
                    { 320, "Item Selected For Putaway, Putaway In Progress" },
                    { 511, "Item Added To Order" },
                    { 521, "Container Selected For Picking" },
                    { 522, "Item Picked Into Container" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 211);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 212);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 320);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 511);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 521);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 522);

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 111,
                column: "EventTypeDescription",
                value: "Container Declared Full");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 112,
                column: "EventTypeDescription",
                value: "Container Registered");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 210,
                column: "EventTypeDescription",
                value: "Item Registered, Waiting To Be Selected For Putaway");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 310,
                column: "EventTypeDescription",
                value: "Item Putaway Into Container Complete");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 410,
                column: "EventTypeDescription",
                value: "Order Registered, Waiting To Be Selected For Picking");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 510,
                column: "EventTypeDescription",
                value: "Order Picking Completed, Waiting Be To Selected For Packaging");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 520,
                column: "EventTypeDescription",
                value: "Order Selected For Packaging, Packaging In Progress");

            migrationBuilder.InsertData(
                table: "EventTypes",
                columns: new[] { "Id", "EventTypeDescription" },
                values: new object[,]
                {
                    { 220, "Item Selected For Putaway, Putaway In Progress" },
                    { 411, "Item Added To Order" },
                    { 420, "Order Selected For Picking, Picking In Progress" },
                    { 421, "Container Selected For Picking" },
                    { 422, "Item Pick From Container Before" },
                    { 423, "Item Pick From Container After" },
                    { 610, "Order Packaging Completed, Waiting To Be Selected For Shipping" },
                    { 620, "Order Selected For Shipping, Shipping Preparation In Progress" },
                    { 710, "Order Shipped" }
                });
        }
    }
}
