using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class addSport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sport",
                table: "sections");

            migrationBuilder.AddColumn<Guid>(
                name: "sport_id",
                table: "sections",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "sports",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sports", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_sections_sport_id",
                table: "sections",
                column: "sport_id");

            migrationBuilder.AddForeignKey(
                name: "fk_sections_sports_sport_id",
                table: "sections",
                column: "sport_id",
                principalTable: "sports",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_sections_sports_sport_id",
                table: "sections");

            migrationBuilder.DropTable(
                name: "sports");

            migrationBuilder.DropIndex(
                name: "ix_sections_sport_id",
                table: "sections");

            migrationBuilder.DropColumn(
                name: "sport_id",
                table: "sections");

            migrationBuilder.AddColumn<string>(
                name: "sport",
                table: "sections",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
