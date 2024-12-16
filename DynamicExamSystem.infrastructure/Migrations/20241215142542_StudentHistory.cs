using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynamicExamSystem.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StudentHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentExams_AspNetUsers_UserId",
                table: "StudentExams");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentExams_Exams_ExamId",
                table: "StudentExams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentExams",
                table: "StudentExams");

            migrationBuilder.RenameTable(
                name: "StudentExams",
                newName: "StudentHistories");

            migrationBuilder.RenameIndex(
                name: "IX_StudentExams_UserId",
                table: "StudentHistories",
                newName: "IX_StudentHistories_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentHistories",
                table: "StudentHistories",
                columns: new[] { "ExamId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentHistories_AspNetUsers_UserId",
                table: "StudentHistories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentHistories_Exams_ExamId",
                table: "StudentHistories",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentHistories_AspNetUsers_UserId",
                table: "StudentHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentHistories_Exams_ExamId",
                table: "StudentHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentHistories",
                table: "StudentHistories");

            migrationBuilder.RenameTable(
                name: "StudentHistories",
                newName: "StudentExams");

            migrationBuilder.RenameIndex(
                name: "IX_StudentHistories_UserId",
                table: "StudentExams",
                newName: "IX_StudentExams_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentExams",
                table: "StudentExams",
                columns: new[] { "ExamId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentExams_AspNetUsers_UserId",
                table: "StudentExams",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentExams_Exams_ExamId",
                table: "StudentExams",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
