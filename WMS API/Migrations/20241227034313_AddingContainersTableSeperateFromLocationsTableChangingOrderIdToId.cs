using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class AddingContainersTableSeperateFromLocationsTableChangingOrderIdToId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ObjectId",
                table: "Orders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ObjectId",
                table: "Locations",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ObjectId",
                table: "Items",
                newName: "Id");

            migrationBuilder.CreateTable(
                name: "Containers",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    EventDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PreviousEventId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    NextEventId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Containers", x => x.EventId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Items_OrderId",
                table: "Items",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Containers");

            migrationBuilder.DropIndex(
                name: "IX_Items_OrderId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Orders",
                newName: "ObjectId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Locations",
                newName: "ObjectId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Items",
                newName: "ObjectId");
        }
    }
}
