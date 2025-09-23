using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistracijaVozila.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class klijentrolechangetosefodsjeka : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2f2ca54-7b0e-4d50-9b8a-7a5f9d6e3a11",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Šef odsjeka", "ŠEF ODSJEKA" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93e88597-7567-4e0a-af32-d3c79597ab97",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "769688f7-b751-4c7f-8f39-fed7151d5c6d", "AQAAAAIAAYagAAAAEAL+t3+HYyIUajPf1Z21zxQPuyJ7q1ld4u2oHZAI3bkx9klZCdQHf5dLB8MaLBvQbQ==", "d90f655e-a837-401f-80fa-b834c40b4336" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2f2ca54-7b0e-4d50-9b8a-7a5f9d6e3a11",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Klijent", "KLIJENT" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93e88597-7567-4e0a-af32-d3c79597ab97",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "67a2d11a-294c-409d-b2fe-8bce912a87bb", "AQAAAAIAAYagAAAAEAG3zJo1GNutrTemVzF/PVMzoyqBykwxMkrjd62ZiIEgMN9uGFRm0UawjzJMeKqFEQ==", "e49e4962-86f1-4796-9c24-c87d902ef8c6" });
        }
    }
}
