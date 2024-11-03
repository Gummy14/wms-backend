using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class ChangingObjectEventIdToJustObjectId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContainerEventId",
                table: "Items",
                newName: "ContainerId");

            migrationBuilder.RenameColumn(
                name: "ItemEventId",
                table: "Containers",
                newName: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContainerId",
                table: "Items",
                newName: "ContainerEventId");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "Containers",
                newName: "ItemEventId");
        }
    }
}
