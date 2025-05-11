using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class AddingEventDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventDescription",
                table: "ShipmentData",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "EventDescription",
                table: "OrderData",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "EventDescription",
                table: "LocationData",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "EventDescription",
                table: "ItemData",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "EventDescription",
                table: "ContainerData",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "EventDescription",
                table: "BoxData",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventDescription",
                table: "ShipmentData");

            migrationBuilder.DropColumn(
                name: "EventDescription",
                table: "OrderData");

            migrationBuilder.DropColumn(
                name: "EventDescription",
                table: "LocationData");

            migrationBuilder.DropColumn(
                name: "EventDescription",
                table: "ItemData");

            migrationBuilder.DropColumn(
                name: "EventDescription",
                table: "ContainerData");

            migrationBuilder.DropColumn(
                name: "EventDescription",
                table: "BoxData");
        }
    }
}
