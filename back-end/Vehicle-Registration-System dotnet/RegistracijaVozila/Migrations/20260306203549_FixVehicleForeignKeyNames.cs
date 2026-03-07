using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleRegistrationSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixVehicleForeignKeyNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TipVozilaId",
                table: "Vehicles",
                newName: "VehicleTypeId");

            migrationBuilder.RenameColumn(
                name: "MarkaVozilaId",
                table: "Vehicles",
                newName: "VehicleBrandId");

            migrationBuilder.RenameColumn(
                name: "ModelVozilaId",
                table: "Vehicles",
                newName: "VehicleModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VehicleTypeId",
                table: "Vehicles",
                newName: "TipVozilaId");

            migrationBuilder.RenameColumn(
                name: "VehicleBrandId",
                table: "Vehicles",
                newName: "MarkaVozilaId");

            migrationBuilder.RenameColumn(
                name: "VehicleModelId",
                table: "Vehicles",
                newName: "ModelVozilaId");
        }
    }
}
