using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI_Resume.Migrations
{
    /// <inheritdoc />
    public partial class AddResumeFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resumes_AspNetUsers_ApplicationUserId",
                table: "Resumes");

            migrationBuilder.DropIndex(
                name: "IX_Resumes_ApplicationUserId",
                table: "Resumes");

            migrationBuilder.DropIndex(
                name: "IX_ResumeAnalyses_ResumeId",
                table: "ResumeAnalyses");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Resumes");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Resumes",
                newName: "FullName");

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Resumes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_UserId",
                table: "Resumes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeAnalyses_ResumeId",
                table: "ResumeAnalyses",
                column: "ResumeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resumes_AspNetUsers_UserId",
                table: "Resumes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resumes_AspNetUsers_UserId",
                table: "Resumes");

            migrationBuilder.DropIndex(
                name: "IX_Resumes_UserId",
                table: "Resumes");

            migrationBuilder.DropIndex(
                name: "IX_ResumeAnalyses_ResumeId",
                table: "ResumeAnalyses");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Resumes");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Resumes",
                newName: "Title");

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Resumes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Resumes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_ApplicationUserId",
                table: "Resumes",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeAnalyses_ResumeId",
                table: "ResumeAnalyses",
                column: "ResumeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Resumes_AspNetUsers_ApplicationUserId",
                table: "Resumes",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
