using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class AddingSubLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Locations_SubLocationId",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "SubLocationId",
                table: "Locations",
                newName: "LocationParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Locations_SubLocationId",
                table: "Locations",
                newName: "IX_Locations_LocationParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Locations_LocationParentId",
                table: "Locations",
                column: "LocationParentId",
                principalTable: "Locations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Locations_LocationParentId",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "LocationParentId",
                table: "Locations",
                newName: "SubLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Locations_LocationParentId",
                table: "Locations",
                newName: "IX_Locations_SubLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Locations_SubLocationId",
                table: "Locations",
                column: "SubLocationId",
                principalTable: "Locations",
                principalColumn: "Id");
        }
    }
}
