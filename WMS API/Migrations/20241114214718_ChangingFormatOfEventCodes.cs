using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class ChangingFormatOfEventCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 2);

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

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 13);

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

            migrationBuilder.InsertData(
                table: "EventTypes",
                columns: new[] { "Id", "EventTypeDescription" },
                values: new object[,]
                {
                    { 110, "Container Registered" },
                    { 210, "Item Registered, Waiting To Be Selected For Putaway" },
                    { 220, "Item Selected For Putaway, Putaway In Progress" },
                    { 310, "Item Putaway Into Container Complete" },
                    { 410, "Order Registered, Waiting To Be Selected For Picking" },
                    { 411, "Item Added To Order" },
                    { 420, "Order Selected For Picking, Picking In Progress" },
                    { 421, "Item Pick From Container Before" },
                    { 422, "Item Pick From Container After" },
                    { 510, "Order Picking Completed, Waiting Be To Selected For Packaging" },
                    { 520, "Order Selected For Packaging, Packaging In Progress" },
                    { 610, "Order Packaging Completed, Waiting To Be Selected For Shipping" },
                    { 620, "Order Selected For Shipping, Shipping Preparation In Progress" },
                    { 710, "Order Shipped" }
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
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 220);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 310);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 410);

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
                keyValue: 510);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 520);

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

            migrationBuilder.InsertData(
                table: "EventTypes",
                columns: new[] { "Id", "EventTypeDescription" },
                values: new object[,]
                {
                    { 1, "Container Registered" },
                    { 2, "Item Registered, Added To Putaway Queue, Waiting For Selection" },
                    { 3, "Item Selected From Putaway Queue, Putaway In Progress" },
                    { 4, "Item Putaway Into Container Complete" },
                    { 5, "Item Added To Order" },
                    { 6, "Item Pick From Container Before" },
                    { 7, "Item Pick From Container After" },
                    { 8, "Order Received, Added To New Orders Queue, Waiting To Be Selected" },
                    { 9, "Order Selected From New Orders Queue, Picking In Progress" },
                    { 10, "Order Picking Completed, Moving To Packaging Queue" },
                    { 11, "Order Added To Packaging Queue, Waiting To Be Selected" },
                    { 12, "Order Selected From Packaging Queue, Packaging In Progress" },
                    { 13, "Order Packaging Completed, Moving To Shipping Queue" },
                    { 14, "Order Added Shipping Queue, Waiting To Be Selected" },
                    { 15, "Order Seleted From Shipping Queue, Shipping Preparation In Progress" },
                    { 16, "Order Shipped" }
                });
        }
    }
}
