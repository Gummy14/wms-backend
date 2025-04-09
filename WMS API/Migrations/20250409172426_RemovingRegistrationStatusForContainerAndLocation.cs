using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class RemovingRegistrationStatusForContainerAndLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 212);

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 110,
                column: "EventTypeDescription",
                value: "Location Unoccupied");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 210,
                column: "EventTypeDescription",
                value: "Container Not In Use");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 110,
                column: "EventTypeDescription",
                value: "Location Newly Registered, Unoccupied");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 210,
                column: "EventTypeDescription",
                value: "Container Newly Registered, Not In Use");

            migrationBuilder.InsertData(
                table: "EventTypes",
                columns: new[] { "Id", "EventTypeDescription" },
                values: new object[,]
                {
                    { 112, "Location Unoccupied" },
                    { 212, "Container Not In Use" }
                });
        }
    }
}
