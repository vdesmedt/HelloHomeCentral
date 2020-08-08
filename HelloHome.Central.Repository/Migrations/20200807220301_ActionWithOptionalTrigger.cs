using Microsoft.EntityFrameworkCore.Migrations;

namespace HelloHome.Central.Repository.Migrations
{
    public partial class ActionWithOptionalTrigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Action_Trigger_TriggerId",
                table: "Action");

            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "Action");

            migrationBuilder.AlterColumn<int>(
                name: "TriggerId",
                table: "Action",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Action_Trigger_TriggerId",
                table: "Action",
                column: "TriggerId",
                principalTable: "Trigger",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Action_Trigger_TriggerId",
                table: "Action");

            migrationBuilder.AlterColumn<int>(
                name: "TriggerId",
                table: "Action",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sequence",
                table: "Action",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Action_Trigger_TriggerId",
                table: "Action",
                column: "TriggerId",
                principalTable: "Trigger",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
