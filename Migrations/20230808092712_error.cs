using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace datingAppreal.Migrations
{
    /// <inheritdoc />
    public partial class error : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Photos");

            migrationBuilder.RenameColumn(
                name: "Intrudction",
                table: "User",
                newName: "Introduction");

            migrationBuilder.RenameColumn(
                name: "Interesets",
                table: "User",
                newName: "Interests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Introduction",
                table: "User",
                newName: "Intrudction");

            migrationBuilder.RenameColumn(
                name: "Interests",
                table: "User",
                newName: "Interesets");

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "Photos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
