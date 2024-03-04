using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainnigApI.Migrations
{
    /// <inheritdoc />
    public partial class AddTranssactionReservationAndListToModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rooms_Centers_CenterId",
                table: "rooms");

            migrationBuilder.DropIndex(
                name: "IX_rooms_CenterId",
                table: "rooms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_rooms_CenterId",
                table: "rooms",
                column: "CenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_rooms_Centers_CenterId",
                table: "rooms",
                column: "CenterId",
                principalTable: "Centers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
