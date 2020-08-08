using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HelloHome.Central.Repository.Migrations
{
    public partial class Scripts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinDelta",
                table: "Trigger");

            migrationBuilder.AddColumn<int>(
                name: "PressStyle",
                table: "Trigger",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Script",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    TriggerId = table.Column<int>(nullable: false),
                    OnFinnishId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Script", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Script_Script_OnFinnishId",
                        column: x => x.OnFinnishId,
                        principalTable: "Script",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Script_Trigger_TriggerId",
                        column: x => x.TriggerId,
                        principalTable: "Trigger",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScriptAction",
                columns: table => new
                {
                    ScriptId = table.Column<int>(nullable: false),
                    ActionId = table.Column<int>(nullable: false),
                    Sequence = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScriptAction", x => new { x.ScriptId, x.ActionId });
                    table.ForeignKey(
                        name: "FK_ScriptAction_Action_ActionId",
                        column: x => x.ActionId,
                        principalTable: "Action",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScriptAction_Script_ScriptId",
                        column: x => x.ScriptId,
                        principalTable: "Script",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Script_OnFinnishId",
                table: "Script",
                column: "OnFinnishId");

            migrationBuilder.CreateIndex(
                name: "IX_Script_TriggerId",
                table: "Script",
                column: "TriggerId");

            migrationBuilder.CreateIndex(
                name: "IX_ScriptAction_ActionId",
                table: "ScriptAction",
                column: "ActionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScriptAction");

            migrationBuilder.DropTable(
                name: "Script");

            migrationBuilder.DropColumn(
                name: "PressStyle",
                table: "Trigger");

            migrationBuilder.AddColumn<int>(
                name: "MinDelta",
                table: "Trigger",
                type: "int",
                nullable: true);
        }
    }
}
