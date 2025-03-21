﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class AddingItemContainerRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Items");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "Containers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "StatusType" },
                values: new object[] { 3, "Picked" });

            migrationBuilder.CreateIndex(
                name: "IX_Containers_ItemId",
                table: "Containers",
                column: "ItemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Containers_Items_ItemId",
                table: "Containers",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Containers_Items_ItemId",
                table: "Containers");

            migrationBuilder.DropIndex(
                name: "IX_Containers_ItemId",
                table: "Containers");

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "Containers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
