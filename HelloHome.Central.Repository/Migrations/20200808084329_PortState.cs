using Microsoft.EntityFrameworkCore.Migrations;

namespace HelloHome.Central.Repository.Migrations
{
    public partial class PortState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SensorState",
                table: "Port");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Port");

            migrationBuilder.AddColumn<int>(
                name: "PortState",
                table: "Port",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PortState",
                table: "Port");

            migrationBuilder.AddColumn<int>(
                name: "SensorState",
                table: "Port",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Port",
                type: "int",
                nullable: true);
        }
    }
}
