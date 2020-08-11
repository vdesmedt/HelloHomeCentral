using Microsoft.EntityFrameworkCore.Migrations;

namespace HelloHome.Central.Repository.Migrations
{
    public partial class PortHistoryFKFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortHistory_Port_PortId",
                table: "PortHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PortHistory_Port_PortId1",
                table: "PortHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PortHistory_Port_PortId2",
                table: "PortHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PortHistory_Port_PortId3",
                table: "PortHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PortHistory_Port_PortId4",
                table: "PortHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PortHistory_Port_PortId5",
                table: "PortHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PortHistory_Port_PortId6",
                table: "PortHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PortHistory_Port_PortId7",
                table: "PortHistory");

            migrationBuilder.DropIndex(
                name: "IX_PortHistory_PortId1",
                table: "PortHistory");

            migrationBuilder.DropIndex(
                name: "IX_PortHistory_PortId2",
                table: "PortHistory");

            migrationBuilder.DropIndex(
                name: "IX_PortHistory_PortId3",
                table: "PortHistory");

            migrationBuilder.DropIndex(
                name: "IX_PortHistory_PortId4",
                table: "PortHistory");

            migrationBuilder.DropIndex(
                name: "IX_PortHistory_PortId5",
                table: "PortHistory");

            migrationBuilder.DropIndex(
                name: "IX_PortHistory_PortId6",
                table: "PortHistory");

            migrationBuilder.DropIndex(
                name: "IX_PortHistory_PortId7",
                table: "PortHistory");

            migrationBuilder.AddForeignKey(
                name: "FK_Port_PortHistory_PortId",
                table: "PortHistory",
                column: "PortId",
                principalTable: "Port",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Port_PortHistory_PortId",
                table: "PortHistory");

            migrationBuilder.CreateIndex(
                name: "IX_PortHistory_PortId1",
                table: "PortHistory",
                column: "PortId");

            migrationBuilder.CreateIndex(
                name: "IX_PortHistory_PortId2",
                table: "PortHistory",
                column: "PortId");

            migrationBuilder.CreateIndex(
                name: "IX_PortHistory_PortId3",
                table: "PortHistory",
                column: "PortId");

            migrationBuilder.CreateIndex(
                name: "IX_PortHistory_PortId4",
                table: "PortHistory",
                column: "PortId");

            migrationBuilder.CreateIndex(
                name: "IX_PortHistory_PortId5",
                table: "PortHistory",
                column: "PortId");

            migrationBuilder.CreateIndex(
                name: "IX_PortHistory_PortId6",
                table: "PortHistory",
                column: "PortId");

            migrationBuilder.CreateIndex(
                name: "IX_PortHistory_PortId7",
                table: "PortHistory",
                column: "PortId");

            migrationBuilder.AddForeignKey(
                name: "FK_PortHistory_Port_PortId",
                table: "PortHistory",
                column: "PortId",
                principalTable: "Port",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PortHistory_Port_PortId1",
                table: "PortHistory",
                column: "PortId",
                principalTable: "Port",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PortHistory_Port_PortId2",
                table: "PortHistory",
                column: "PortId",
                principalTable: "Port",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PortHistory_Port_PortId3",
                table: "PortHistory",
                column: "PortId",
                principalTable: "Port",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PortHistory_Port_PortId4",
                table: "PortHistory",
                column: "PortId",
                principalTable: "Port",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PortHistory_Port_PortId5",
                table: "PortHistory",
                column: "PortId",
                principalTable: "Port",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PortHistory_Port_PortId6",
                table: "PortHistory",
                column: "PortId",
                principalTable: "Port",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PortHistory_Port_PortId7",
                table: "PortHistory",
                column: "PortId",
                principalTable: "Port",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
