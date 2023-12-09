using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelloHome.Central.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UniqueIndexOnPortNameNodeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Port_Node_NodeId", table: "Port");

            migrationBuilder.CreateIndex(
                name: "IX_Port_NodeId_Name",
                table: "Port",
                columns: new[] { "NodeId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Port_Node_NodeId", table: "Port");

            migrationBuilder.DropIndex(
                name: "IX_Port_NodeId_Name",
                table: "Port");

            migrationBuilder.AddForeignKey(
                name: "FK_Port_Node_NodeId",
                table:"Port",
                column: "NodeId",
                principalTable: "Node",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
