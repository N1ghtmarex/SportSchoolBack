using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropColumn(
                name: "room",
                table: "sections");

            migrationBuilder.AddColumn<Guid>(
                name: "room_id",
                table: "sections",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    external_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    surname = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "coachs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    external_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    surname = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_coachs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ClientSection",
                columns: table => new
                {
                    client_id = table.Column<Guid>(type: "uuid", nullable: false),
                    section_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_section", x => new { x.client_id, x.section_id });
                    table.ForeignKey(
                        name: "fk_client_section_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_client_section_sections_section_id",
                        column: x => x.section_id,
                        principalTable: "sections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_sections_coach_id",
                table: "sections",
                column: "coach_id");

            migrationBuilder.CreateIndex(
                name: "ix_sections_room_id",
                table: "sections",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_section_section_id",
                table: "ClientSection",
                column: "section_id");

            migrationBuilder.AddForeignKey(
                name: "fk_sections_coachs_coach_id",
                table: "sections",
                column: "coach_id",
                principalTable: "coachs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_sections_rooms_room_id",
                table: "sections",
                column: "room_id",
                principalTable: "rooms",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_sections_coachs_coach_id",
                table: "sections");

            migrationBuilder.DropForeignKey(
                name: "fk_sections_rooms_room_id",
                table: "sections");

            migrationBuilder.DropTable(
                name: "ClientSection");

            migrationBuilder.DropTable(
                name: "coachs");

            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.DropIndex(
                name: "ix_sections_coach_id",
                table: "sections");

            migrationBuilder.DropIndex(
                name: "ix_sections_room_id",
                table: "sections");

            migrationBuilder.DropColumn(
                name: "room_id",
                table: "sections");

            migrationBuilder.AddColumn<string>(
                name: "room",
                table: "sections",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    external_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    surname = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });
        }
    }
}
