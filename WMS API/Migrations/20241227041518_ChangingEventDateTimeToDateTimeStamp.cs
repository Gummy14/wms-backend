using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class ChangingEventDateTimeToDateTimeStamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EventDateTime",
                table: "Orders",
                newName: "DateTimeStamp");

            migrationBuilder.RenameColumn(
                name: "EventDateTime",
                table: "Locations",
                newName: "DateTimeStamp");

            migrationBuilder.RenameColumn(
                name: "EventDateTime",
                table: "Items",
                newName: "DateTimeStamp");

            migrationBuilder.RenameColumn(
                name: "EventDateTime",
                table: "Containers",
                newName: "DateTimeStamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTimeStamp",
                table: "Orders",
                newName: "EventDateTime");

            migrationBuilder.RenameColumn(
                name: "DateTimeStamp",
                table: "Locations",
                newName: "EventDateTime");

            migrationBuilder.RenameColumn(
                name: "DateTimeStamp",
                table: "Items",
                newName: "EventDateTime");

            migrationBuilder.RenameColumn(
                name: "DateTimeStamp",
                table: "Containers",
                newName: "EventDateTime");
        }
    }
}
