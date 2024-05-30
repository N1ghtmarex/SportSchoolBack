using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class enchCoach : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "education_form",
                table: "coachs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "faculty",
                table: "coachs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "institution",
                table: "coachs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "job",
                table: "coachs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "job_period",
                table: "coachs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "job_title",
                table: "coachs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "qualification",
                table: "coachs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "speciality",
                table: "coachs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "education_form",
                table: "coachs");

            migrationBuilder.DropColumn(
                name: "faculty",
                table: "coachs");

            migrationBuilder.DropColumn(
                name: "institution",
                table: "coachs");

            migrationBuilder.DropColumn(
                name: "job",
                table: "coachs");

            migrationBuilder.DropColumn(
                name: "job_period",
                table: "coachs");

            migrationBuilder.DropColumn(
                name: "job_title",
                table: "coachs");

            migrationBuilder.DropColumn(
                name: "qualification",
                table: "coachs");

            migrationBuilder.DropColumn(
                name: "speciality",
                table: "coachs");
        }
    }
}
