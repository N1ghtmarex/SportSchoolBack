using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class indEventAddRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "room_id",
                table: "individual_events",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_individual_events_room_id",
                table: "individual_events",
                column: "room_id");

            migrationBuilder.AddForeignKey(
                name: "fk_individual_events_rooms_room_id",
                table: "individual_events",
                column: "room_id",
                principalTable: "rooms",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_individual_events_rooms_room_id",
                table: "individual_events");

            migrationBuilder.DropIndex(
                name: "ix_individual_events_room_id",
                table: "individual_events");

            migrationBuilder.DropColumn(
                name: "room_id",
                table: "individual_events");
        }
    }
}
