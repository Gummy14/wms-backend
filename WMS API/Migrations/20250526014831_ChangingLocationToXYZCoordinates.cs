using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class ChangingLocationToXYZCoordinates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Column",
                table: "LocationData");

            migrationBuilder.RenameColumn(
                name: "Zone",
                table: "LocationData",
                newName: "ZCoordinate");

            migrationBuilder.RenameColumn(
                name: "Shelf",
                table: "LocationData",
                newName: "YCoordinate");

            migrationBuilder.RenameColumn(
                name: "Row",
                table: "LocationData",
                newName: "XCoordinate");

            migrationBuilder.AddColumn<Guid>(
                name: "SubLocationId",
                table: "Locations",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_SubLocationId",
                table: "Locations",
                column: "SubLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Locations_SubLocationId",
                table: "Locations",
                column: "SubLocationId",
                principalTable: "Locations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Locations_SubLocationId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_SubLocationId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "SubLocationId",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "ZCoordinate",
                table: "LocationData",
                newName: "Zone");

            migrationBuilder.RenameColumn(
                name: "YCoordinate",
                table: "LocationData",
                newName: "Shelf");

            migrationBuilder.RenameColumn(
                name: "XCoordinate",
                table: "LocationData",
                newName: "Row");

            migrationBuilder.AddColumn<int>(
                name: "Column",
                table: "LocationData",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
