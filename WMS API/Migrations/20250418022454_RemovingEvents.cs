using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class RemovingEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventTypes");

            migrationBuilder.DropColumn(
                name: "EventType",
                table: "OrderData");

            migrationBuilder.DropColumn(
                name: "EventType",
                table: "LocationData");

            migrationBuilder.DropColumn(
                name: "EventType",
                table: "ItemData");

            migrationBuilder.DropColumn(
                name: "EventType",
                table: "ContainerData");

            migrationBuilder.DropColumn(
                name: "EventType",
                table: "BoxData");

            migrationBuilder.AddColumn<bool>(
                name: "Acknowledged",
                table: "OrderData",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Acknowledged",
                table: "OrderData");

            migrationBuilder.AddColumn<int>(
                name: "EventType",
                table: "OrderData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventType",
                table: "LocationData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventType",
                table: "ItemData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventType",
                table: "ContainerData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventType",
                table: "BoxData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EventTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EventTypeDescription = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "EventTypes",
                columns: new[] { "Id", "EventTypeDescription" },
                values: new object[,]
                {
                    { 110, "Location Unoccupied" },
                    { 111, "Location Occupied" },
                    { 210, "Container Not In Use" },
                    { 211, "Container In Use" },
                    { 310, "Item Registered" },
                    { 410, "Item Putaway Into Location" },
                    { 510, "Order Newly Registered, Waiting To Be Selected For Picking" },
                    { 511, "Item Added To Order" },
                    { 520, "Order Selected For Picking, Picking In Progress" },
                    { 522, "Item Picked Into Container" }
                });
        }
    }
}
