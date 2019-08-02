using Microsoft.EntityFrameworkCore.Migrations;

namespace LookO2.Importer.Persistance.Migrations
{
    public partial class ExtendMetersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Hcho",
                table: "Readings",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Humidity",
                table: "Readings",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Temerature",
                table: "Readings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hcho",
                table: "Readings");

            migrationBuilder.DropColumn(
                name: "Humidity",
                table: "Readings");

            migrationBuilder.DropColumn(
                name: "Temerature",
                table: "Readings");
        }
    }
}
