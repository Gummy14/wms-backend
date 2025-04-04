using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_API.Migrations
{
    /// <inheritdoc />
    public partial class AddingBoxFieldsToItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BoxId",
                table: "Items",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "BoxName",
                table: "Items",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoxId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "BoxName",
                table: "Items");
        }
    }
}
