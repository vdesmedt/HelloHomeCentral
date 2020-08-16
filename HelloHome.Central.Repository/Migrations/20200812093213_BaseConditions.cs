using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HelloHome.Central.Repository.Migrations
{
    public partial class BaseConditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Script",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Condition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Type = table.Column<string>(unicode: false, maxLength: 2, nullable: false),
                    PortId = table.Column<int>(nullable: true),
                    MinTemperature = table.Column<float>(nullable: true),
                    MaxTemperature = table.Column<float>(nullable: true),
                    From = table.Column<TimeSpan>(nullable: true),
                    To = table.Column<TimeSpan>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Condition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Condition_Port_PortId",
                        column: x => x.PortId,
                        principalTable: "Port",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScriptCondition",
                columns: table => new
                {
                    ScriptId = table.Column<int>(nullable: false),
                    ConditionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScriptCondition", x => new { x.ScriptId, x.ConditionId });
                    table.ForeignKey(
                        name: "FK_ScriptCondition_Condition_ConditionId",
                        column: x => x.ConditionId,
                        principalTable: "Condition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScriptCondition_Script_ScriptId",
                        column: x => x.ScriptId,
                        principalTable: "Script",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Condition_PortId",
                table: "Condition",
                column: "PortId");

            migrationBuilder.CreateIndex(
                name: "IX_ScriptCondition_ConditionId",
                table: "ScriptCondition",
                column: "ConditionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScriptCondition");

            migrationBuilder.DropTable(
                name: "Condition");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Script",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
