using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace exam_api_project.Migrations
{
    /// <inheritdoc />
    public partial class addedUniqueSSN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Patients_SocialSecurityNumber",
                table: "Patients",
                column: "SocialSecurityNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patients_SocialSecurityNumber",
                table: "Patients");
        }
    }
}
