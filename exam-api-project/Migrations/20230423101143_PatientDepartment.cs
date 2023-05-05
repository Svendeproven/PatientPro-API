using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace exam_api_project.Migrations
{
    /// <inheritdoc />
    public partial class PatientDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentModelId",
                table: "Patients",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_DepartmentModelId",
                table: "Patients",
                column: "DepartmentModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Departments_DepartmentModelId",
                table: "Patients",
                column: "DepartmentModelId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Departments_DepartmentModelId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_DepartmentModelId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "DepartmentModelId",
                table: "Patients");
        }
    }
}
