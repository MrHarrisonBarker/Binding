using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Binding.Migrations
{
    public partial class createdandupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Users",
                nullable: false,
                defaultValue: DateTime.Now);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "Users",
                nullable: false,
                defaultValue: DateTime.Now);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Pages",
                nullable: false,
                defaultValue: DateTime.Now);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "Pages",
                nullable: false,
                defaultValue: DateTime.Now);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Blocks",
                nullable: false,
                defaultValue: DateTime.Now);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "Blocks",
                nullable: false,
                defaultValue: DateTime.Now);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Blocks");
        }
    }
}
