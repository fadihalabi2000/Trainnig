using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainnigApI.Migrations
{
    /// <inheritdoc />
    public partial class updatetableresrvationservice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFree",
                table: "services");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "reservationServices");

            migrationBuilder.DropColumn(
                name: "CostPerPersonPerDay",
                table: "reservations");

            migrationBuilder.RenameColumn(
                name: "StayDurationDays",
                table: "reservationServices",
                newName: "DurationDays");

            migrationBuilder.AddColumn<bool>(
                name: "IsFree",
                table: "reservationServices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "UnitPrice",
                table: "reservationServices",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFree",
                table: "reservationServices");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "reservationServices");

            migrationBuilder.RenameColumn(
                name: "DurationDays",
                table: "reservationServices",
                newName: "StayDurationDays");

            migrationBuilder.AddColumn<bool>(
                name: "IsFree",
                table: "services",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "reservationServices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CostPerPersonPerDay",
                table: "reservations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
