using Microsoft.EntityFrameworkCore.Migrations;

namespace HelloHome.Central.Repository.Migrations
{
    public partial class AddForeignKeyOnSensorTrigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trigger_Port_SensorPortId",
                table: "Trigger");

            migrationBuilder.AddForeignKey(
                name: "FK_Trigger_Port_SensorPortId",
                table: "Trigger",
                column: "SensorPortId",
                principalTable: "Port",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trigger_Port_SensorPortId",
                table: "Trigger");

            migrationBuilder.AddForeignKey(
                name: "FK_Trigger_Port_SensorPortId",
                table: "Trigger",
                column: "SensorPortId",
                principalTable: "Port",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
