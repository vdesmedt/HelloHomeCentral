using Microsoft.EntityFrameworkCore.Migrations;

namespace HelloHome.Central.Repository.Migrations
{
    public partial class ActionDiscrToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(name: "tmp", table: "Action", nullable: false);
            migrationBuilder.Sql("UPDATE Action SET tmp='SC' WHERE Type=1;");
            migrationBuilder.Sql("UPDATE Action SET tmp='AA' WHERE Type=10;");
            migrationBuilder.Sql("UPDATE Action SET tmp='RA' WHERE Type=100;");
            migrationBuilder.Sql("UPDATE Action SET tmp='TN' WHERE Type=101;");
            migrationBuilder.Sql("UPDATE Action SET tmp='TF' WHERE Type=102;");
            migrationBuilder.Sql("UPDATE Action SET Type=0;");
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Action",
                unicode: false,
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
            migrationBuilder.Sql("UPDATE Action SET Type=tmp;");
            migrationBuilder.DropColumn("tmp", "Action");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(name: "tmp", table: "Action", nullable: false);
            migrationBuilder.Sql("UPDATE Action SET tmp=1 WHERE Type='SC';");
            migrationBuilder.Sql("UPDATE Action SET tmp=10 WHERE Type='AA';");
            migrationBuilder.Sql("UPDATE Action SET tmp=100 WHERE Type='RA';");
            migrationBuilder.Sql("UPDATE Action SET tmp=101 WHERE Type='TN';");
            migrationBuilder.Sql("UPDATE Action SET tmp=102 WHERE Type='TF';");
            migrationBuilder.Sql("UPDATE Action SET Type='0';");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Action",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 2);
            migrationBuilder.Sql("UPDATE Action SET Type=tmp;");
            migrationBuilder.DropColumn("tmp", "Action");
        }
    }
}