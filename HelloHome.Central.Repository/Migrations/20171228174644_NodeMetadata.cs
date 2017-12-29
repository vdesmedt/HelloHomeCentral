using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HelloHome.Central.Repository.Migrations
{
    public partial class NodeMetadata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Metadata_Name",
                table: "Node",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Metadata_Version",
                table: "Node",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Metadata_Name",
                table: "Node");

            migrationBuilder.DropColumn(
                name: "Metadata_Version",
                table: "Node");
        }
    }
}
