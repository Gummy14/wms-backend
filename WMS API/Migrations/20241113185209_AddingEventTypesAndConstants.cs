using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class AddingEventTypesAndConstants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "EventTypeDescription",
                value: "Container Registered");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "EventTypeDescription",
                value: "Item Registered, Added To Putaway Queue");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "EventTypeDescription",
                value: "Item Selected From Putaway Queue, Putaway In Progress");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "EventTypeDescription",
                value: "Item Putaway Into Container Complete");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "EventTypeDescription",
                value: "Item Added To Order");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "EventTypeDescription",
                value: "Item Pick From Container Before");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "EventTypeDescription",
                value: "Item Pick From Container After");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 8,
                column: "EventTypeDescription",
                value: "Order Received, Added To New Orders Queue, Waiting To Be Selected");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 9,
                column: "EventTypeDescription",
                value: "Order Selected From New Orders Queue, Picking In Progress");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 10,
                column: "EventTypeDescription",
                value: "Order Picking Completed, Moving To Packaging Queue");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 11,
                column: "EventTypeDescription",
                value: "Order Added To Packaging Queue, Waiting To Be Selected");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 12,
                column: "EventTypeDescription",
                value: "Order Selected From Packaging Queue, Packaging In Progress");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 13,
                column: "EventTypeDescription",
                value: "Order Packaging Completed, Moving To Shipping Queue");

            migrationBuilder.InsertData(
                table: "EventTypes",
                columns: new[] { "Id", "EventTypeDescription" },
                values: new object[,]
                {
                    { 14, "Order Added Shipping Queue, Waiting To Be Selected" },
                    { 15, "Order Seleted From Shipping Queue, Shipping Preparation In Progress" },
                    { 16, "Order Shipped" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "EventTypeDescription",
                value: "Item Registered");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "EventTypeDescription",
                value: "Container Registered");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "EventTypeDescription",
                value: "Item Putaway Into Container");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "EventTypeDescription",
                value: "Item Added To Order");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "EventTypeDescription",
                value: "Item Pick From Container Before");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "EventTypeDescription",
                value: "Item Pick From Container After");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "EventTypeDescription",
                value: "Order Received");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 8,
                column: "EventTypeDescription",
                value: "Order Acknowledged, Picking In Progress");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 9,
                column: "EventTypeDescription",
                value: "Order Picking Complete, Enroute To Packaging");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 10,
                column: "EventTypeDescription",
                value: "Order At Packaging Station, Packaging In Progress");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 11,
                column: "EventTypeDescription",
                value: "Order Packaging Complete, Enroute To Shipping");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 12,
                column: "EventTypeDescription",
                value: "Order At Shipping Station, Shipping Preperation In Progress");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 13,
                column: "EventTypeDescription",
                value: "Order Shipped");
        }
    }
}
