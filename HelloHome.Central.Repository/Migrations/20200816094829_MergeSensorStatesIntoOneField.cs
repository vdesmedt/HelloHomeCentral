using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HelloHome.Central.Repository.Migrations
{
    public partial class MergeSensorStatesIntoOneField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE PortHistory SET NewSensorState=PressStyle WHERE Discr=4;");
            migrationBuilder.DropColumn(
                name: "PressStyle",
                table: "PortHistory");

            migrationBuilder.Sql("UPDATE PortHistory SET NewSensorState=NewLevel WHERE Discr=6;");
            migrationBuilder.DropColumn(
                name: "NewLevel",
                table: "PortHistory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PressStyle",
                table: "PortHistory",
                type: "int",
                nullable: true);
            migrationBuilder.Sql("UPDATE PortHistory SET PressStyle=NewSensorState WHERE Discr=4;");


            migrationBuilder.AddColumn<int>(
                name: "NewLevel",
                table: "PortHistory",
                type: "int",
                nullable: true);
            migrationBuilder.Sql("UPDATE PortHistory SET NewLevel=NewSensorState WHERE Discr=6;");
        }
    }
}
