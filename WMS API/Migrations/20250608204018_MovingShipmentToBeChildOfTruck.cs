using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class MovingShipmentToBeChildOfTruck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoxData_Trucks_TruckId",
                table: "BoxData");

            migrationBuilder.DropForeignKey(
                name: "FK_Trucks_Shipments_ShipmentId",
                table: "Trucks");

            migrationBuilder.DropIndex(
                name: "IX_Trucks_ShipmentId",
                table: "Trucks");

            migrationBuilder.DropIndex(
                name: "IX_BoxData_TruckId",
                table: "BoxData");

            migrationBuilder.DropColumn(
                name: "ShipmentId",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "TruckId",
                table: "BoxData");

            migrationBuilder.AddColumn<Guid>(
                name: "TruckId",
                table: "ShipmentData",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentData_TruckId",
                table: "ShipmentData",
                column: "TruckId");

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
                name: "FK_ShipmentData_Trucks_TruckId",
                table: "ShipmentData");

            migrationBuilder.DropIndex(
                name: "IX_ShipmentData_TruckId",
                table: "ShipmentData");

            migrationBuilder.DropColumn(
                name: "TruckId",
                table: "ShipmentData");

            migrationBuilder.AddColumn<Guid>(
                name: "ShipmentId",
                table: "Trucks",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TruckId",
                table: "BoxData",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_ShipmentId",
                table: "Trucks",
                column: "ShipmentId");

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
                name: "FK_Trucks_Shipments_ShipmentId",
                table: "Trucks",
                column: "ShipmentId",
                principalTable: "Shipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
