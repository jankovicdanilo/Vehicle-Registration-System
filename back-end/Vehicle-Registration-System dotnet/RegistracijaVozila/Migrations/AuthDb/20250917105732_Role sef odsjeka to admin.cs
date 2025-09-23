using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistracijaVozila.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class Rolesefodsjekatoadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "f2f2ca54-7b0e-4d50-9b8a-7a5f9d6e3a11", "93e88597-7567-4e0a-af32-d3c79597ab97" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93e88597-7567-4e0a-af32-d3c79597ab97",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "164d3d02-bad8-47d5-afaf-2a05fcf8811a", "AQAAAAIAAYagAAAAEClli48hlBuidmOcdRujEjbnAQH3dgQaqm1o4ozGRrrWlhrT+MWEYepTwbPigjgQsA==", "badaad80-f3ea-4bc9-bd6c-659e6dc9034e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f2f2ca54-7b0e-4d50-9b8a-7a5f9d6e3a11", "93e88597-7567-4e0a-af32-d3c79597ab97" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93e88597-7567-4e0a-af32-d3c79597ab97",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "769688f7-b751-4c7f-8f39-fed7151d5c6d", "AQAAAAIAAYagAAAAEAL+t3+HYyIUajPf1Z21zxQPuyJ7q1ld4u2oHZAI3bkx9klZCdQHf5dLB8MaLBvQbQ==", "d90f655e-a837-401f-80fa-b834c40b4336" });
        }
    }
}
