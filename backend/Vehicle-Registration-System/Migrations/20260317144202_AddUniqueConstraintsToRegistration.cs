using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleRegistrationSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintsToRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LicensePlate",
                table: "Registrations",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: new Guid("30303030-3030-3030-3030-303030303030"),
                columns: new[] { "EngineCapacity", "EnginePowerKw" },
                values: new object[] { 150f, 15 });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: new Guid("40404040-4040-4040-4040-404040404040"),
                column: "EnginePowerKw",
                value: 45);

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_LicensePlate",
                table: "Registrations",
                column: "LicensePlate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Registrations_LicensePlate",
                table: "Registrations");

            migrationBuilder.AlterColumn<string>(
                name: "LicensePlate",
                table: "Registrations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: new Guid("30303030-3030-3030-3030-303030303030"),
                columns: new[] { "EngineCapacity", "EnginePowerKw" },
                values: new object[] { 1000f, 250 });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: new Guid("40404040-4040-4040-4040-404040404040"),
                column: "EnginePowerKw",
                value: 103);
        }
    }
}
