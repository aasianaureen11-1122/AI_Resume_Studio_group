using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI_Resume.Migrations
{
    /// <inheritdoc />
    public partial class AddAIFieldsToResume : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AIGeneratedResume",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AIImprovements",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AIScore",
                table: "Resumes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AISkillGaps",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AISummary",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AIGeneratedResume",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "AIImprovements",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "AIScore",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "AISkillGaps",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "AISummary",
                table: "Resumes");
        }
    }
}
