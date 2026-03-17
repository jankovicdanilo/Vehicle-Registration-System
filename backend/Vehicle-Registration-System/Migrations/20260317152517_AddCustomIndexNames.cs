using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleRegistrationSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomIndexNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Registrations_VehicleId",
                table: "Registrations",
                newName: "UQ_Registration_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_Registrations_LicensePlate",
                table: "Registrations",
                newName: "UQ_Registration_LicensePlate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "UQ_Registration_VehicleId",
                table: "Registrations",
                newName: "IX_Registrations_VehicleId");

            migrationBuilder.RenameIndex(
                name: "UQ_Registration_LicensePlate",
                table: "Registrations",
                newName: "IX_Registrations_LicensePlate");
        }
    }
}
