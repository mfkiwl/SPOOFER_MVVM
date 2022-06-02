using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Spoofer.Migrations
{
    public partial class initializerize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Coordinates");

            migrationBuilder.AddColumn<int>(
                name: "NumberInOrder",
                table: "Coordinates",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberInOrder",
                table: "Coordinates");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Coordinates",
                type: "datetime2",
                nullable: true);
        }
    }
}
