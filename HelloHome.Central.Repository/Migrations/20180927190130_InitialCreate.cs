﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HelloHome.Central.Repository.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Node",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Signature = table.Column<long>(nullable: false),
                    RfNetwork = table.Column<byte>(nullable: false),
                    RfAddress = table.Column<byte>(nullable: false),
                    LastSeen = table.Column<DateTime>(nullable: false),
                    NodeType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    EmonCmsNodeId = table.Column<int>(nullable: true),
                    ExtraFeatures = table.Column<short>(nullable: false),
                    NodeInfoFrequency = table.Column<byte>(nullable: false),
                    EnvironmentFrequency = table.Column<byte>(nullable: false),
                    Version = table.Column<string>(maxLength: 10, nullable: true),
                    VIn = table.Column<float>(nullable: true),
                    SendErrorCount = table.Column<int>(nullable: false),
                    Temperature = table.Column<float>(nullable: true),
                    Humidity = table.Column<float>(nullable: true),
                    AtmosphericPressure = table.Column<float>(nullable: true),
                    Rssi = table.Column<int>(nullable: false),
                    NodeStartCount = table.Column<int>(nullable: false),
                    StartupTime = table.Column<DateTime>(nullable: false),
                    MaxUpTimeRaw = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Node", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "nodelog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NodeId = table.Column<int>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    Type = table.Column<string>(maxLength: 10, nullable: false),
                    Data = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nodelog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_nodelog_Node_NodeId",
                        column: x => x.NodeId,
                        principalTable: "Node",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Port",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NodeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Type = table.Column<int>(nullable: false),
                    PulseCount = table.Column<int>(nullable: true),
                    State = table.Column<bool>(nullable: true),
                    Value = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Port", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Port_Node_NodeId",
                        column: x => x.NodeId,
                        principalTable: "Node",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NodeHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NodeId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    Rssi = table.Column<int>(nullable: false),
                    Discr = table.Column<int>(nullable: false),
                    Temperature = table.Column<float>(nullable: true),
                    Humidity = table.Column<float>(nullable: true),
                    Pressure = table.Column<int>(nullable: true),
                    VIn = table.Column<float>(nullable: true),
                    SendErrorCount = table.Column<int>(nullable: true),
                    PulseSensorPortId = table.Column<int>(nullable: true),
                    NewPulses = table.Column<int>(nullable: true),
                    Total = table.Column<int>(nullable: true),
                    IsOffset = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodeHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NodeHistory_Node_NodeId",
                        column: x => x.NodeId,
                        principalTable: "Node",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NodeHistory_Port_PulseSensorPortId",
                        column: x => x.PulseSensorPortId,
                        principalTable: "Port",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trigger",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(nullable: false),
                    CronExpression = table.Column<string>(maxLength: 20, nullable: true),
                    SensorPortId = table.Column<int>(nullable: true),
                    TriggerOnState = table.Column<bool>(nullable: true),
                    MinDelta = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trigger", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trigger_Port_SensorPortId",
                        column: x => x.SensorPortId,
                        principalTable: "Port",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Action",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TriggerId = table.Column<int>(nullable: false),
                    Sequence = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    ActuatorId = table.Column<int>(nullable: true),
                    Delay = table.Column<TimeSpan>(nullable: true),
                    ScheduledActionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Action", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Action_Trigger_TriggerId",
                        column: x => x.TriggerId,
                        principalTable: "Trigger",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Action_Port_ActuatorId",
                        column: x => x.ActuatorId,
                        principalTable: "Port",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Action_Action_ScheduledActionId",
                        column: x => x.ScheduledActionId,
                        principalTable: "Action",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Action_TriggerId",
                table: "Action",
                column: "TriggerId");

            migrationBuilder.CreateIndex(
                name: "IX_Action_ActuatorId",
                table: "Action",
                column: "ActuatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Action_ScheduledActionId",
                table: "Action",
                column: "ScheduledActionId");

            migrationBuilder.CreateIndex(
                name: "IX_Node_RfAddress",
                table: "Node",
                column: "RfAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NodeHistory_NodeId",
                table: "NodeHistory",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_NodeHistory_PulseSensorPortId",
                table: "NodeHistory",
                column: "PulseSensorPortId");

            migrationBuilder.CreateIndex(
                name: "IX_nodelog_NodeId",
                table: "nodelog",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Port_NodeId",
                table: "Port",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Trigger_SensorPortId",
                table: "Trigger",
                column: "SensorPortId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Action");

            migrationBuilder.DropTable(
                name: "NodeHistory");

            migrationBuilder.DropTable(
                name: "nodelog");

            migrationBuilder.DropTable(
                name: "Trigger");

            migrationBuilder.DropTable(
                name: "Port");

            migrationBuilder.DropTable(
                name: "Node");
        }
    }
}
