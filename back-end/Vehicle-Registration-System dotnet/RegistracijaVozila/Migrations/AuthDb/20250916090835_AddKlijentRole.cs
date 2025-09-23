using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistracijaVozila.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class AddKlijentRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f2f2ca54-7b0e-4d50-9b8a-7a5f9d6e3a11", "f2f2ca54-7b0e-4d50-9b8a-7a5f9d6e3a11", "Klijent", "KLIJENT" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93e88597-7567-4e0a-af32-d3c79597ab97",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "67a2d11a-294c-409d-b2fe-8bce912a87bb", "AQAAAAIAAYagAAAAEAG3zJo1GNutrTemVzF/PVMzoyqBykwxMkrjd62ZiIEgMN9uGFRm0UawjzJMeKqFEQ==", "e49e4962-86f1-4796-9c24-c87d902ef8c6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2f2ca54-7b0e-4d50-9b8a-7a5f9d6e3a11");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93e88597-7567-4e0a-af32-d3c79597ab97",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "33c90c88-b3ea-4817-aad9-b97d6b07c0c9", "AQAAAAIAAYagAAAAEE40hl+lt+NiofVN8FKrkjbJKh0Hrcnq+qSpVp9vAs1IK6bZyDB8l+QXBslAbduvrA==", "096e36b8-1f3f-4f56-a611-dfbae42cf2e3" });
        }
    }
}
