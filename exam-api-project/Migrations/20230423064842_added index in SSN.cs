using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace exam_api_project.Migrations
{
    /// <inheritdoc />
    public partial class addedindexinSSN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Patients_SocialSecurityNumber",
                table: "Patients",
                newName: "IX_Patient_SocialSecurityNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Patient_SocialSecurityNumber",
                table: "Patients",
                newName: "IX_Patients_SocialSecurityNumber");
        }
    }
}
