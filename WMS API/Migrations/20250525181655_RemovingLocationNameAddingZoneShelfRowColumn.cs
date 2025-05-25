using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class RemovingLocationNameAddingZoneShelfRowColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "LocationData");

            migrationBuilder.AddColumn<int>(
                name: "Column",
                table: "LocationData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Row",
                table: "LocationData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Shelf",
                table: "LocationData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Zone",
                table: "LocationData",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Column",
                table: "LocationData");

            migrationBuilder.DropColumn(
                name: "Row",
                table: "LocationData");

            migrationBuilder.DropColumn(
                name: "Shelf",
                table: "LocationData");

            migrationBuilder.DropColumn(
                name: "Zone",
                table: "LocationData");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "LocationData",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
