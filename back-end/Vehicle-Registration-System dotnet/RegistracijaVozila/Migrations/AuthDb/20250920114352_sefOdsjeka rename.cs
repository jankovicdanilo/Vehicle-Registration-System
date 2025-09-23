using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistracijaVozila.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class sefOdsjekarename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { "164d3d02-bad8-47d5-afaf-2a05fcf8811a", "AQAAAAIAAYagAAAAEClli48hlBuidmOcdRujEjbnAQH3dgQaqm1o4ozGRrrWlhrT+MWEYepTwbPigjgQsA==", "badaad80-f3ea-4bc9-bd6c-659e6dc9034e" });
        }
    }
}
