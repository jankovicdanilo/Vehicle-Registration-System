using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleRegistrationSystem.Migrations
{
    /// <inheritdoc />
    public partial class karoserijaremoval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Karoserija",
                table: "Vozila");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Karoserija",
                table: "Vozila",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
