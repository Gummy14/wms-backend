using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class AddingContainerType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContainerType",
                table: "ContainerDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 421,
                column: "EventTypeDescription",
                value: "Container Selected For Picking");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 422,
                column: "EventTypeDescription",
                value: "Item Pick From Container Before");

            migrationBuilder.InsertData(
                table: "EventTypes",
                columns: new[] { "Id", "EventTypeDescription" },
                values: new object[] { 423, "Item Pick From Container After" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 423);

            migrationBuilder.DropColumn(
                name: "ContainerType",
                table: "ContainerDetails");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 421,
                column: "EventTypeDescription",
                value: "Item Pick From Container Before");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 422,
                column: "EventTypeDescription",
                value: "Item Pick From Container After");
        }
    }
}
