using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace exam_api_project.Migrations
{
    /// <inheritdoc />
    public partial class Fixadmispelling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tittle",
                table: "Medicines",
                newName: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Medicines",
                newName: "Tittle");
        }
    }
}