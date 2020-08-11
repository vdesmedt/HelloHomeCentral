using Microsoft.EntityFrameworkCore.Migrations;

namespace HelloHome.Central.Repository.Migrations
{
    public partial class HasOne2OwnsOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NodeType",
                table: "Node",
                newName: "Metadata_NodeType");

            migrationBuilder.RenameColumn(
                name: "ExtraFeatures",
                table: "Node",
                newName: "Metadata_ExtraFeatures");

            migrationBuilder.RenameColumn(
                name: "VIn",
                table: "Node",
                newName: "AggregatedData_VIn");

            migrationBuilder.RenameColumn(
                name: "Temperature",
                table: "Node",
                newName: "AggregatedData_Temperature");

            migrationBuilder.RenameColumn(
                name: "StartupTime",
                table: "Node",
                newName: "AggregatedData_StartupTime");

            migrationBuilder.RenameColumn(
                name: "SendErrorCount",
                table: "Node",
                newName: "AggregatedData_SendErrorCount");

            migrationBuilder.RenameColumn(
                name: "Rssi",
                table: "Node",
                newName: "AggregatedData_Rssi");

            migrationBuilder.RenameColumn(
                name: "NodeStartCount",
                table: "Node",
                newName: "AggregatedData_NodeStartCount");

            migrationBuilder.RenameColumn(
                name: "MaxUpTimeRaw",
                table: "Node",
                newName: "AggregatedData_MaxUpTimeRaw");

            migrationBuilder.RenameColumn(
                name: "Humidity",
                table: "Node",
                newName: "AggregatedData_Humidity");

            migrationBuilder.RenameColumn(
                name: "AtmosphericPressure",
                table: "Node",
                newName: "AggregatedData_AtmosphericPressure");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Metadata_NodeType",
                table: "Node",
                newName: "NodeType");

            migrationBuilder.RenameColumn(
                name: "Metadata_ExtraFeatures",
                table: "Node",
                newName: "ExtraFeatures");

            migrationBuilder.RenameColumn(
                name: "AggregatedData_VIn",
                table: "Node",
                newName: "VIn");

            migrationBuilder.RenameColumn(
                name: "AggregatedData_Temperature",
                table: "Node",
                newName: "Temperature");

            migrationBuilder.RenameColumn(
                name: "AggregatedData_StartupTime",
                table: "Node",
                newName: "StartupTime");

            migrationBuilder.RenameColumn(
                name: "AggregatedData_SendErrorCount",
                table: "Node",
                newName: "SendErrorCount");

            migrationBuilder.RenameColumn(
                name: "AggregatedData_Rssi",
                table: "Node",
                newName: "Rssi");

            migrationBuilder.RenameColumn(
                name: "AggregatedData_NodeStartCount",
                table: "Node",
                newName: "NodeStartCount");

            migrationBuilder.RenameColumn(
                name: "AggregatedData_MaxUpTimeRaw",
                table: "Node",
                newName: "MaxUpTimeRaw");

            migrationBuilder.RenameColumn(
                name: "AggregatedData_Humidity",
                table: "Node",
                newName: "Humidity");

            migrationBuilder.RenameColumn(
                name: "AggregatedData_AtmosphericPressure",
                table: "Node",
                newName: "AtmosphericPressure");
        }
    }
}
