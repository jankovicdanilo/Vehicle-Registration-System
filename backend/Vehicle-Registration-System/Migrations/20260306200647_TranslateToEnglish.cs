using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleRegistrationSystem.Migrations
{
    /// <inheritdoc />
    public partial class TranslateToEnglish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Klijenti",
                newName: "Clients");

            migrationBuilder.RenameTable(
                name: "Osiguranja",
                newName: "Insurances");

            migrationBuilder.RenameTable(
                name: "TipoviVozila",
                newName: "VehicleTypes");

            migrationBuilder.RenameTable(
                name: "MarkeVozila",
                newName: "VehicleBrands");

            migrationBuilder.RenameTable(
                name: "ModeliVozila",
                newName: "VehicleModels");

            migrationBuilder.RenameTable(
                name: "Vozila",
                newName: "Vehicles");

            migrationBuilder.RenameTable(
                name: "Registracije",
                newName: "Registrations");

            migrationBuilder.RenameTable(
                name: "OsiguranjeCijene",
                newName: "InsurancePrices");


            migrationBuilder.RenameColumn(
                name: "Ime",
                table: "Clients",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "Prezime",
                table: "Clients",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "JMBG",
                table: "Clients",
                newName: "NationalId");

            migrationBuilder.RenameColumn(
                name: "BrojLicneKarte",
                table: "Clients",
                newName: "IdCardNumber");

            migrationBuilder.RenameColumn(
                name: "BrojTelefona",
                table: "Clients",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "Adresa",
                table: "Clients",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "DatumRodjenja",
                table: "Clients",
                newName: "DateOfBirth");


            migrationBuilder.RenameColumn(
                name: "Naziv",
                table: "Insurances",
                newName: "Name");


            migrationBuilder.RenameColumn(
                name: "Naziv",
                table: "VehicleTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Kategorija",
                table: "VehicleTypes",
                newName: "Category");


            migrationBuilder.RenameColumn(
                name: "Naziv",
                table: "VehicleBrands",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "TipVozilaId",
                table: "VehicleBrands",
                newName: "VehicleTypeId");


            migrationBuilder.RenameColumn(
                name: "Naziv",
                table: "VehicleModels",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "MarkaVozilaId",
                table: "VehicleModels",
                newName: "VehicleBrandId");

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

            migrationBuilder.RenameColumn(
                name: "GodinaProizvodnje",
                table: "Vehicles",
                newName: "ProductionYear");

            migrationBuilder.RenameColumn(
                name: "ZapreminaMotora",
                table: "Vehicles",
                newName: "EngineCapacity");

            migrationBuilder.RenameColumn(
                name: "SnagaMotora",
                table: "Vehicles",
                newName: "EnginePowerKw");

            migrationBuilder.RenameColumn(
                name: "Masa",
                table: "Vehicles",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "VrstaGoriva",
                table: "Vehicles",
                newName: "FuelType");

            migrationBuilder.RenameColumn(
                name: "BrojSasije",
                table: "Vehicles",
                newName: "ChassisNumber");

            migrationBuilder.RenameColumn(
                name: "DatumPrveRegistracije",
                table: "Vehicles",
                newName: "FirstRegistrationDate");


            migrationBuilder.RenameColumn(
                name: "DatumRegistracije",
                table: "Registrations",
                newName: "RegistrationDate");

            migrationBuilder.RenameColumn(
                name: "DatumIstekaRegistracije",
                table: "Registrations",
                newName: "ExpirationDate");

            migrationBuilder.RenameColumn(
                name: "CijenaRegistracije",
                table: "Registrations",
                newName: "RegistrationPrice");

            migrationBuilder.RenameColumn(
                name: "RegistarskaOznaka",
                table: "Registrations",
                newName: "LicensePlate");

            migrationBuilder.RenameColumn(
                name: "PrivremenaRegistracija",
                table: "Registrations",
                newName: "IsTemporary");

            migrationBuilder.RenameColumn(
                name: "KlijentId",
                table: "Registrations",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "VoziloId",
                table: "Registrations",
                newName: "VehicleId");

            migrationBuilder.RenameColumn(
                name: "OsiguranjeId",
                table: "Registrations",
                newName: "InsuranceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Clients",
                newName: "Klijenti");

            migrationBuilder.RenameTable(
                name: "Insurances",
                newName: "Osiguranja");

            migrationBuilder.RenameTable(
                name: "VehicleTypes",
                newName: "TipoviVozila");

            migrationBuilder.RenameTable(
                name: "VehicleBrands",
                newName: "MarkeVozila");

            migrationBuilder.RenameTable(
                name: "VehicleModels",
                newName: "ModeliVozila");

            migrationBuilder.RenameTable(
                name: "Vehicles",
                newName: "Vozila");

            migrationBuilder.RenameTable(
                name: "Registrations",
                newName: "Registracije");

            migrationBuilder.RenameTable(
                name: "InsurancePrices",
                newName: "OsiguranjeCijene");


            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Klijenti",
                newName: "Ime");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Klijenti",
                newName: "Prezime");

            migrationBuilder.RenameColumn(
                name: "NationalId",
                table: "Klijenti",
                newName: "JMBG");

            migrationBuilder.RenameColumn(
                name: "IdCardNumber",
                table: "Klijenti",
                newName: "BrojLicneKarte");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Klijenti",
                newName: "BrojTelefona");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Klijenti",
                newName: "Adresa");

            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "Klijenti",
                newName: "DatumRodjenja");
        }
    }
}
