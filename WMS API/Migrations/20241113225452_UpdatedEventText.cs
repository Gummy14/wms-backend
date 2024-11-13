using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedEventText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "EventTypeDescription",
                value: "Item Registered, Added To Putaway Queue, Waiting For Selection");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "EventTypeDescription",
                value: "Item Registered, Added To Putaway Queue");
        }
    }
}
