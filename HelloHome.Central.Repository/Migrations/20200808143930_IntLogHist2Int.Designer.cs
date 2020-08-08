﻿// <auto-generated />
using System;
using HelloHome.Central.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HelloHome.Central.Repository.Migrations
{
    [DbContext(typeof(HhDbContext))]
    [Migration("20200808143930_IntLogHist2Int")]
    partial class IntLogHist2Int
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.Action", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("TriggerId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TriggerId");

                    b.ToTable("Action");

                    b.HasDiscriminator<int>("Type");
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.Node", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("LastSeen")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("RfAddress")
                        .HasColumnType("int");

                    b.Property<long>("Signature")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("RfAddress")
                        .IsUnique();

                    b.ToTable("Node");
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.NodeAggregatedData", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<float?>("AtmosphericPressure")
                        .HasColumnType("float");

                    b.Property<float?>("Humidity")
                        .HasColumnType("float");

                    b.Property<float>("MaxUpTimeRaw")
                        .HasColumnType("float");

                    b.Property<int>("NodeStartCount")
                        .HasColumnType("int");

                    b.Property<int>("Rssi")
                        .HasColumnType("int");

                    b.Property<int>("SendErrorCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartupTime")
                        .HasColumnType("datetime(6)");

                    b.Property<float?>("Temperature")
                        .HasColumnType("float");

                    b.Property<float?>("VIn")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Node");
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.NodeLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Data")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4")
                        .HasMaxLength(255);

                    b.Property<int>("NodeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar(10) CHARACTER SET utf8mb4")
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.HasIndex("NodeId");

                    b.ToTable("nodelog");
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.NodeMetadata", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<short>("ExtraFeatures")
                        .HasColumnType("smallint");

                    b.Property<string>("Name")
                        .HasColumnName("Name")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<int>("NodeType")
                        .HasColumnType("int");

                    b.Property<string>("Version")
                        .HasColumnName("Version")
                        .HasColumnType("varchar(10) CHARACTER SET utf8mb4")
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.ToTable("Node");
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.Port", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<int>("NodeId")
                        .HasColumnType("int");

                    b.Property<byte>("PortNumber")
                        .HasColumnType("tinyint unsigned");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NodeId");

                    b.ToTable("Port");

                    b.HasDiscriminator<int>("Type");
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.PortHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Discr")
                        .HasColumnType("int");

                    b.Property<int>("PortId")
                        .HasColumnType("int");

                    b.Property<int>("Rssi")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("PortHistory");

                    b.HasDiscriminator<int>("Discr");
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.Script", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("OnFinnishId")
                        .HasColumnType("int");

                    b.Property<int>("TriggerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OnFinnishId");

                    b.HasIndex("TriggerId");

                    b.ToTable("Script");
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.ScriptAction", b =>
                {
                    b.Property<int>("ScriptId")
                        .HasColumnType("int");

                    b.Property<int>("ActionId")
                        .HasColumnType("int");

                    b.Property<int>("Sequence")
                        .HasColumnType("int");

                    b.HasKey("ScriptId", "ActionId");

                    b.HasIndex("ActionId");

                    b.ToTable("ScriptAction");
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.Trigger", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Trigger");

                    b.HasDiscriminator<int>("Type");
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.ActuatorAction", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.Action");

                    b.Property<int?>("ActuatorId")
                        .HasColumnType("int");

                    b.HasIndex("ActuatorId");

                    b.HasDiscriminator().HasValue(10);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.ScheduleAction", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.Action");

                    b.Property<TimeSpan>("Delay")
                        .HasColumnType("time(6)");

                    b.Property<int?>("ScheduledActionId")
                        .HasColumnType("int");

                    b.HasIndex("ScheduledActionId");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.ActuatorPort", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.Port");

                    b.HasDiscriminator();
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.LoggingPort", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.Port");

                    b.HasDiscriminator();
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.SensorPort", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.Port");

                    b.HasDiscriminator();
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.EnvironmentHistory", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.PortHistory");

                    b.Property<float?>("Humidity")
                        .HasColumnType("float");

                    b.Property<float?>("Pressure")
                        .HasColumnType("float");

                    b.Property<float?>("Temperature")
                        .HasColumnType("float");

                    b.HasIndex("PortId");

                    b.HasDiscriminator().HasValue(3);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.FloatDataLogPortHistory", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.PortHistory");

                    b.Property<float>("Data")
                        .HasColumnName("FloatLogData")
                        .HasColumnType("float");

                    b.HasIndex("PortId")
                        .HasName("IX_PortHistory_PortId1");

                    b.HasDiscriminator().HasValue(8);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.IntDataLogPortHistory", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.PortHistory");

                    b.Property<int>("Data")
                        .HasColumnName("IntLogData")
                        .HasColumnType("int");

                    b.HasIndex("PortId")
                        .HasName("IX_PortHistory_PortId2");

                    b.HasDiscriminator().HasValue(7);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.NodeHealthHistory", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.PortHistory");

                    b.Property<int>("SendErrorCount")
                        .HasColumnType("int");

                    b.Property<float?>("VIn")
                        .HasColumnType("float");

                    b.HasIndex("PortId")
                        .HasName("IX_PortHistory_PortId3");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.PulseHistory", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.PortHistory");

                    b.Property<bool>("IsOffset")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("NewPulses")
                        .HasColumnType("int");

                    b.Property<int>("Total")
                        .HasColumnType("int");

                    b.HasIndex("PortId")
                        .HasName("IX_PortHistory_PortId4");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.PushButtonHistory", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.PortHistory");

                    b.Property<int>("PressStyle")
                        .HasColumnType("int");

                    b.HasIndex("PortId")
                        .HasName("IX_PortHistory_PortId5");

                    b.HasDiscriminator().HasValue(4);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.SwitchPortHistory", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.PortHistory");

                    b.Property<int>("NewSensorState")
                        .HasColumnType("int");

                    b.HasIndex("PortId")
                        .HasName("IX_PortHistory_PortId6");

                    b.HasDiscriminator().HasValue(5);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.VarioButtonPortHistory", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.PortHistory");

                    b.Property<int>("NewLevel")
                        .HasColumnType("int");

                    b.HasIndex("PortId")
                        .HasName("IX_PortHistory_PortId7");

                    b.HasDiscriminator().HasValue(6);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.CronTrigger", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.Trigger");

                    b.Property<string>("CronExpression")
                        .HasColumnType("varchar(20) CHARACTER SET utf8mb4")
                        .HasMaxLength(20);

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.SensorTrigger", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.Trigger");

                    b.Property<int>("SensorPortId")
                        .HasColumnType("int");

                    b.HasIndex("SensorPortId");

                    b.HasDiscriminator();
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.RelayAction", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.ActuatorAction");

                    b.HasDiscriminator().HasValue(100);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.RelayActuatorPort", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.ActuatorPort");

                    b.Property<int>("RelayState")
                        .HasColumnName("PortState")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue(7);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.FloatDataLogPort", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.LoggingPort");

                    b.Property<float>("Data")
                        .HasColumnName("FloatLogData")
                        .HasColumnType("float");

                    b.HasDiscriminator().HasValue(8);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.IntDataLogPort", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.LoggingPort");

                    b.Property<int>("Data")
                        .HasColumnName("IntLogData")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue(9);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.EnvironmentSensorPort", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.SensorPort");

                    b.Property<float>("AtmPressure")
                        .HasColumnType("float");

                    b.Property<float>("Humidity")
                        .HasColumnType("float");

                    b.Property<float>("Temperature")
                        .HasColumnType("float");

                    b.Property<int>("UpdateFrequency")
                        .HasColumnName("EnvUpdateFreq")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.NodeHealthSensorPort", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.SensorPort");

                    b.Property<int>("SendError")
                        .HasColumnType("int");

                    b.Property<int>("UpdateFrequency")
                        .HasColumnName("HealtUpdateFreq")
                        .HasColumnType("int");

                    b.Property<float>("VIn")
                        .HasColumnType("float");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.PulseSensorPort", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.SensorPort");

                    b.Property<int>("PulseCount")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue(3);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.PushSensorPort", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.SensorPort");

                    b.Property<int>("LastPressStyle")
                        .HasColumnName("PortState")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue(4);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.SwitchSensorPort", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.SensorPort");

                    b.Property<int>("SwitchState")
                        .HasColumnName("PortState")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue(5);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.VarioSensorPort", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.SensorPort");

                    b.Property<int>("Level")
                        .HasColumnName("PortState")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue(6);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.PushTrigger", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.SensorTrigger");

                    b.Property<int?>("PressStyle")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.SwitchTrigger", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.SensorTrigger");

                    b.Property<bool?>("TriggerOnState")
                        .HasColumnType("tinyint(1)");

                    b.HasDiscriminator().HasValue(3);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.VarioTrigger", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.SensorTrigger");

                    b.HasDiscriminator().HasValue(4);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.TurnOffAction", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.RelayAction");

                    b.HasDiscriminator().HasValue(102);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.TurnOnAction", b =>
                {
                    b.HasBaseType("HelloHome.Central.Domain.Entities.RelayAction");

                    b.HasDiscriminator().HasValue(101);
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.Action", b =>
                {
                    b.HasOne("HelloHome.Central.Domain.Entities.Trigger", "Trigger")
                        .WithMany("Actions")
                        .HasForeignKey("TriggerId");
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.NodeAggregatedData", b =>
                {
                    b.HasOne("HelloHome.Central.Domain.Entities.Node", "Node")
                        .WithOne("AggregatedData")
                        .HasForeignKey("HelloHome.Central.Domain.Entities.NodeAggregatedData", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.NodeLog", b =>
                {
                    b.HasOne("HelloHome.Central.Domain.Entities.Node", null)
                        .WithMany("Logs")
                        .HasForeignKey("NodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.NodeMetadata", b =>
                {
                    b.HasOne("HelloHome.Central.Domain.Entities.Node", "Node")
                        .WithOne("Metadata")
                        .HasForeignKey("HelloHome.Central.Domain.Entities.NodeMetadata", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.Port", b =>
                {
                    b.HasOne("HelloHome.Central.Domain.Entities.Node", "Node")
                        .WithMany("Ports")
                        .HasForeignKey("NodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.Script", b =>
                {
                    b.HasOne("HelloHome.Central.Domain.Entities.Script", "OnFinnish")
                        .WithMany()
                        .HasForeignKey("OnFinnishId");

                    b.HasOne("HelloHome.Central.Domain.Entities.Trigger", "Trigger")
                        .WithMany("Scripts")
                        .HasForeignKey("TriggerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.ScriptAction", b =>
                {
                    b.HasOne("HelloHome.Central.Domain.Entities.Action", "Action")
                        .WithMany()
                        .HasForeignKey("ActionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HelloHome.Central.Domain.Entities.Script", "Script")
                        .WithMany("Actions")
                        .HasForeignKey("ScriptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.ActuatorAction", b =>
                {
                    b.HasOne("HelloHome.Central.Domain.Entities.ActuatorPort", "Actuator")
                        .WithMany()
                        .HasForeignKey("ActuatorId");
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.ScheduleAction", b =>
                {
                    b.HasOne("HelloHome.Central.Domain.Entities.Action", "ScheduledAction")
                        .WithMany()
                        .HasForeignKey("ScheduledActionId");
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.EnvironmentHistory", b =>
                {
                    b.HasOne("HelloHome.Central.Domain.Entities.EnvironmentSensorPort", "Port")
                        .WithMany("History")
                        .HasForeignKey("PortId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.FloatDataLogPortHistory", b =>
                {
                    b.HasOne("HelloHome.Central.Domain.Entities.FloatDataLogPort", "Port")
                        .WithMany("History")
                        .HasForeignKey("PortId")
                        .HasConstraintName("FK_PortHistory_Port_PortId1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.IntDataLogPortHistory", b =>
                {
                    b.HasOne("HelloHome.Central.Domain.Entities.IntDataLogPort", "Port")
                        .WithMany("History")
                        .HasForeignKey("PortId")
                        .HasConstraintName("FK_PortHistory_Port_PortId2")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.NodeHealthHistory", b =>
                {
                    b.HasOne("HelloHome.Central.Domain.Entities.NodeHealthSensorPort", "Port")
                        .WithMany("History")
                        .HasForeignKey("PortId")
                        .HasConstraintName("FK_PortHistory_Port_PortId3")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.PulseHistory", b =>
                {
                    b.HasOne("HelloHome.Central.Domain.Entities.PulseSensorPort", "Port")
                        .WithMany("History")
                        .HasForeignKey("PortId")
                        .HasConstraintName("FK_PortHistory_Port_PortId4")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.PushButtonHistory", b =>
                {
                    b.HasOne("HelloHome.Central.Domain.Entities.PushSensorPort", "Port")
                        .WithMany("History")
                        .HasForeignKey("PortId")
                        .HasConstraintName("FK_PortHistory_Port_PortId5")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.SwitchPortHistory", b =>
                {
                    b.HasOne("HelloHome.Central.Domain.Entities.SwitchSensorPort", "Port")
                        .WithMany("History")
                        .HasForeignKey("PortId")
                        .HasConstraintName("FK_PortHistory_Port_PortId6")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.VarioButtonPortHistory", b =>
                {
                    b.HasOne("HelloHome.Central.Domain.Entities.VarioSensorPort", "Port")
                        .WithMany("History")
                        .HasForeignKey("PortId")
                        .HasConstraintName("FK_PortHistory_Port_PortId7")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.SensorTrigger", b =>
                {
                    b.HasOne("HelloHome.Central.Domain.Entities.SensorPort", "SensorPort")
                        .WithMany("Triggers")
                        .HasForeignKey("SensorPortId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
