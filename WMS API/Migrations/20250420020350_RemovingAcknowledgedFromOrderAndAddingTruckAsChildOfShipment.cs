using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class RemovingAcknowledgedFromOrderAndAddingTruckAsChildOfShipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Acknowledged",
                table: "OrderData");

            migrationBuilder.AddColumn<Guid>(
                name: "ShipmentId",
                table: "Trucks",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_ShipmentId",
                table: "Trucks",
                column: "ShipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trucks_Shipments_ShipmentId",
                table: "Trucks",
                column: "ShipmentId",
                principalTable: "Shipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trucks_Shipments_ShipmentId",
                table: "Trucks");

            migrationBuilder.DropIndex(
                name: "IX_Trucks_ShipmentId",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "ShipmentId",
                table: "Trucks");

            migrationBuilder.AddColumn<Guid>(
                name: "TruckId",
                table: "ShipmentData",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<bool>(
                name: "Acknowledged",
                table: "OrderData",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

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
    }
}
