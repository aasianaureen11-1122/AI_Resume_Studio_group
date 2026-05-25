using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI_Resume.Migrations
{
    /// <inheritdoc />
    public partial class FixTemplateNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCurrent",
                table: "WorkExperiences",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "WorkExperiences",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedIn",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Template",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Educations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCurrent",
                table: "WorkExperiences");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "WorkExperiences");

            migrationBuilder.DropColumn(
                name: "LinkedIn",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "Template",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Educations");
        }
    }
}
