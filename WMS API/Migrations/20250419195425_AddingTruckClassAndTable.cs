using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class AddingTruckClassAndTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TruckId",
                table: "ShipmentData",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TruckId",
                table: "BoxData",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "Trucks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LicensePlate = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArrivalDateTimeStamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DepartureDateTimeStamp = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentData_TruckId",
                table: "ShipmentData",
                column: "TruckId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxData_TruckId",
                table: "BoxData",
                column: "TruckId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoxData_Trucks_TruckId",
                table: "BoxData",
                column: "TruckId",
                principalTable: "Trucks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShipmentData_Trucks_TruckId",
                table: "ShipmentData",
                column: "TruckId",
                principalTable: "Trucks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoxData_Trucks_TruckId",
                table: "BoxData");

            migrationBuilder.DropForeignKey(
                name: "FK_ShipmentData_Trucks_TruckId",
                table: "ShipmentData");

            migrationBuilder.DropTable(
                name: "Trucks");

            migrationBuilder.DropIndex(
                name: "IX_ShipmentData_TruckId",
                table: "ShipmentData");

            migrationBuilder.DropIndex(
                name: "IX_BoxData_TruckId",
                table: "BoxData");

            migrationBuilder.DropColumn(
                name: "TruckId",
                table: "ShipmentData");

            migrationBuilder.DropColumn(
                name: "TruckId",
                table: "BoxData");
        }
    }
}
