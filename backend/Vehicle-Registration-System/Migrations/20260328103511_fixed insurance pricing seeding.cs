using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VehicleRegistrationSystem.Migrations
{
    /// <inheritdoc />
    public partial class fixedinsurancepricingseeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "InsurancePrices",
                keyColumn: "Id",
                keyValue: new Guid("70707070-7070-7070-7070-707070707070"),
                column: "MinKw",
                value: 1);

            migrationBuilder.InsertData(
                table: "InsurancePrices",
                columns: new[] { "Id", "InsuranceId", "MaxKw", "MinKw", "PricePerKw" },
                values: new object[,]
                {
                    { new Guid("156f70c9-94bf-40e6-ad5b-8efa662f3a22"), new Guid("60606060-6060-6060-6060-606060606060"), 50, 1, 225m },
                    { new Guid("8dade321-ff73-43f0-9cf0-2c3ec1ce0700"), new Guid("60606060-6060-6060-6060-606060606060"), 100, 51, 265m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InsurancePrices",
                keyColumn: "Id",
                keyValue: new Guid("156f70c9-94bf-40e6-ad5b-8efa662f3a22"));

            migrationBuilder.DeleteData(
                table: "InsurancePrices",
                keyColumn: "Id",
                keyValue: new Guid("8dade321-ff73-43f0-9cf0-2c3ec1ce0700"));

            migrationBuilder.UpdateData(
                table: "InsurancePrices",
                keyColumn: "Id",
                keyValue: new Guid("70707070-7070-7070-7070-707070707070"),
                column: "MinKw",
                value: 0);
        }
    }
}
