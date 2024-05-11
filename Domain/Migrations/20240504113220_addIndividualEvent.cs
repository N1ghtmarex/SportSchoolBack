using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class addIndividualEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "individual_events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    coach_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sport_id = table.Column<Guid>(type: "uuid", nullable: false),
                    client_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_individual_events", x => x.id);
                    table.ForeignKey(
                        name: "fk_individual_events_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_individual_events_coachs_coach_id",
                        column: x => x.coach_id,
                        principalTable: "coachs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_individual_events_sports_sport_id",
                        column: x => x.sport_id,
                        principalTable: "sports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_individual_events_client_id",
                table: "individual_events",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_individual_events_coach_id",
                table: "individual_events",
                column: "coach_id");

            migrationBuilder.CreateIndex(
                name: "ix_individual_events_sport_id",
                table: "individual_events",
                column: "sport_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "individual_events");
        }
    }
}
