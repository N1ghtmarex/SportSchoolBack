using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class sad1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_sections_image_file_image_file_id",
                table: "sections");

            migrationBuilder.DropTable(
                name: "image_file");

            migrationBuilder.DropIndex(
                name: "ix_sections_image_file_id",
                table: "sections");

            migrationBuilder.DropColumn(
                name: "image_file_id",
                table: "sections");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "image_file_id",
                table: "sections",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "image_file",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_image_file", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_sections_image_file_id",
                table: "sections",
                column: "image_file_id");

            migrationBuilder.AddForeignKey(
                name: "fk_sections_image_file_image_file_id",
                table: "sections",
                column: "image_file_id",
                principalTable: "image_file",
                principalColumn: "id");
        }
    }
}
