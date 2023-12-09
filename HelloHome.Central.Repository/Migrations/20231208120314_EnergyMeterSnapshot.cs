using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelloHome.Central.Repository.Migrations
{
    /// <inheritdoc />
    public partial class EnergyMeterSnapshot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PulsePerUnitRatio",
                table: "Port",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Unit",
                table: "Port",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Identifier",
                table: "Node",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EnergyMeterSnapshot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Snapshot = table.Column<double>(type: "double", nullable: false),
                    Discr = table.Column<int>(type: "int", nullable: false),
                    PulseCount = table.Column<int>(type: "int", nullable: true),
                    PortId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergyMeterSnapshot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnergyMeterSnapshot_Port_PortId",
                        column: x => x.PortId,
                        principalTable: "Port",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Node_Identifier",
                table: "Node",
                column: "Identifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EnergyMeterSnapshot_PortId",
                table: "EnergyMeterSnapshot",
                column: "PortId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnergyMeterSnapshot");

            migrationBuilder.DropIndex(
                name: "IX_Node_Identifier",
                table: "Node");

            migrationBuilder.DropColumn(
                name: "PulsePerUnitRatio",
                table: "Port");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Port");

            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "Node");
        }
    }
}
