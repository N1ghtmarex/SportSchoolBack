using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class changePicType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image",
                table: "sections");

            migrationBuilder.AddColumn<string>(
                name: "image_path",
                table: "sections",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_path",
                table: "sections");

            migrationBuilder.AddColumn<byte[]>(
                name: "image",
                table: "sections",
                type: "bytea",
                nullable: true);
        }
    }
}
