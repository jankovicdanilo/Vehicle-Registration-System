using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleRegistrationSystem.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class AuthDbEnglish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "49b719cf-9ebf-4151-af09-835f9c03f6b6",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Employee", "EMPLOYEE" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2f2ca54-7b0e-4d50-9b8a-7a5f9d6e3a11",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Manager", "MANAGER" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93e88597-7567-4e0a-af32-d3c79597ab97",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e234f8a5-9f09-4fe2-8a72-843a6e9e2e3a", "AQAAAAIAAYagAAAAENUlx+tPk9PoXImr/4I1wKq8FoedcLgs/JZp6T/ecKZE7PLfH/q5tGah/U79ohTYEg==", "b156b53a-c5ac-40ac-8a52-9af07b768682" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "49b719cf-9ebf-4151-af09-835f9c03f6b6",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Zaposleni", "ZAPOSLENI" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2f2ca54-7b0e-4d50-9b8a-7a5f9d6e3a11",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "SefOdsjeka", "SEFODSJEKA" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93e88597-7567-4e0a-af32-d3c79597ab97",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dbc0d818-a294-43c2-b296-5c6aef87151d", "AQAAAAIAAYagAAAAEJ6vz1P3h5Gvm+whTKOFRippzBeKBA+WcuEkHT89VbQ2IM301BA4Qlb2Lbpt5kp7uw==", "d57f56d5-b08b-4ce8-8468-3d67f01b9dcd" });
        }
    }
}
