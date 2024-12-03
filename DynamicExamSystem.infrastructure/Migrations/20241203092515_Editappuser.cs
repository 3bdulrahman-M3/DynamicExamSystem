using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynamicExamSystem.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Editappuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Score",
                table: "ExamResults",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "ExamResults");
        }
    }
}
