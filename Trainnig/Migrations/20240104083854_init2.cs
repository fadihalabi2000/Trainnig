using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainnigApI.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reservationServices_reservationRooms_ReservationRoomID",
                table: "reservationServices");

            migrationBuilder.DropIndex(
                name: "IX_reservationServices_ReservationRoomID",
                table: "reservationServices");

            migrationBuilder.DropColumn(
                name: "ReservationRoomID",
                table: "reservationServices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReservationRoomID",
                table: "reservationServices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_reservationServices_ReservationRoomID",
                table: "reservationServices",
                column: "ReservationRoomID");

            migrationBuilder.AddForeignKey(
                name: "FK_reservationServices_reservationRooms_ReservationRoomID",
                table: "reservationServices",
                column: "ReservationRoomID",
                principalTable: "reservationRooms",
                principalColumn: "ID");
        }
    }
}
