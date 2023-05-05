using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace exam_api_project.Migrations
{
    /// <inheritdoc />
    public partial class AddedPatientToPatientTodo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientTodos_Users_UserModelId",
                table: "PatientTodos");

            migrationBuilder.AlterColumn<int>(
                name: "UserModelId",
                table: "PatientTodos",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "PatientModelId",
                table: "PatientTodos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PatientTodos_PatientModelId",
                table: "PatientTodos",
                column: "PatientModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTodos_Patients_PatientModelId",
                table: "PatientTodos",
                column: "PatientModelId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTodos_Users_UserModelId",
                table: "PatientTodos",
                column: "UserModelId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientTodos_Patients_PatientModelId",
                table: "PatientTodos");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientTodos_Users_UserModelId",
                table: "PatientTodos");

            migrationBuilder.DropIndex(
                name: "IX_PatientTodos_PatientModelId",
                table: "PatientTodos");

            migrationBuilder.DropColumn(
                name: "PatientModelId",
                table: "PatientTodos");

            migrationBuilder.AlterColumn<int>(
                name: "UserModelId",
                table: "PatientTodos",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTodos_Users_UserModelId",
                table: "PatientTodos",
                column: "UserModelId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
