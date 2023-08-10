using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace datingAppreal.Migrations
{
    /// <inheritdoc />
    public partial class photo228 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Creted",
                table: "User",
                newName: "Created");

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "Photos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Photos");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "User",
                newName: "Creted");
        }
    }
}
