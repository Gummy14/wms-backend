using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class RemovingContainers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemData_Containers_ContainerId",
                table: "ItemData");

            migrationBuilder.DropTable(
                name: "ContainerData");

            migrationBuilder.DropTable(
                name: "Containers");

            migrationBuilder.DropIndex(
                name: "IX_ItemData_ContainerId",
                table: "ItemData");

            migrationBuilder.DropColumn(
                name: "ContainerId",
                table: "ItemData");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ContainerId",
                table: "ItemData",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "Containers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Containers", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ContainerData",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ContainerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DateTimeStamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EventDescription = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NextEventId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    OrderId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    PrevEventId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContainerData", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_ContainerData_Containers_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "Containers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContainerData_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ItemData_ContainerId",
                table: "ItemData",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_ContainerData_ContainerId",
                table: "ContainerData",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_ContainerData_OrderId",
                table: "ContainerData",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemData_Containers_ContainerId",
                table: "ItemData",
                column: "ContainerId",
                principalTable: "Containers",
                principalColumn: "Id");
        }
    }
}
