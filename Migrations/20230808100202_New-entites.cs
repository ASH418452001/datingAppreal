using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace datingAppreal.Migrations
{
    /// <inheritdoc />
    public partial class Newentites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KnowAs",
                table: "User",
                newName: "KnownAs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KnownAs",
                table: "User",
                newName: "KnowAs");
        }
    }
}
