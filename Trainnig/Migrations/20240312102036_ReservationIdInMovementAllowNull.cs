using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainnigApI.Migrations
{
    /// <inheritdoc />
    public partial class ReservationIdInMovementAllowNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_accountMovements_reservations_ReservationId",
                table: "accountMovements");

            migrationBuilder.AlterColumn<int>(
                name: "ReservationId",
                table: "accountMovements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_accountMovements_reservations_ReservationId",
                table: "accountMovements",
                column: "ReservationId",
                principalTable: "reservations",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_accountMovements_reservations_ReservationId",
                table: "accountMovements");

            migrationBuilder.AlterColumn<int>(
                name: "ReservationId",
                table: "accountMovements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_accountMovements_reservations_ReservationId",
                table: "accountMovements",
                column: "ReservationId",
                principalTable: "reservations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
