using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class AddingDimensionsAndWeightToItemAndLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 320);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 523);

            migrationBuilder.RenameColumn(
                name: "Width",
                table: "Boxes",
                newName: "WidthInCentimeters");

            migrationBuilder.RenameColumn(
                name: "Length",
                table: "Boxes",
                newName: "LengthInCentimeters");

            migrationBuilder.RenameColumn(
                name: "Height",
                table: "Boxes",
                newName: "HeightInCentimeters");

            migrationBuilder.AddColumn<float>(
                name: "HeightInCentimeters",
                table: "Locations",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "LengthInCentimeters",
                table: "Locations",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "MaxWeightInKilograms",
                table: "Locations",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "WidthInCentimeters",
                table: "Locations",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "HeightInCentimeters",
                table: "Items",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "LengthInCentimeters",
                table: "Items",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "WeightInKilograms",
                table: "Items",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "WidthInCentimeters",
                table: "Items",
                type: "float",
                nullable: false,
                defaultValue: 0f);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 521);

            migrationBuilder.DropColumn(
                name: "HeightInCentimeters",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "LengthInCentimeters",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "MaxWeightInKilograms",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "WidthInCentimeters",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "HeightInCentimeters",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "LengthInCentimeters",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "WeightInKilograms",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "WidthInCentimeters",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "WidthInCentimeters",
                table: "Boxes",
                newName: "Width");

            migrationBuilder.RenameColumn(
                name: "LengthInCentimeters",
                table: "Boxes",
                newName: "Length");

            migrationBuilder.RenameColumn(
                name: "HeightInCentimeters",
                table: "Boxes",
                newName: "Height");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 522,
                column: "EventTypeDescription",
                value: "Container Selected For Picking");

            migrationBuilder.InsertData(
                table: "EventTypes",
                columns: new[] { "Id", "EventTypeDescription" },
                values: new object[,]
                {
                    { 320, "Item Selected For Putaway, Putaway In Progress" },
                    { 523, "Item Picked Into Container" }
                });
        }
    }
}
