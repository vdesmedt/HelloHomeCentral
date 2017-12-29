﻿// <auto-generated />
using HelloHome.Central.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace HelloHome.Central.Repository.Migrations
{
    [DbContext(typeof(HelloHomeContext))]
    partial class HelloHomeContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.Node", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("LastSeen");

                    b.Property<DateTimeOffset>("LastStartupTime");

                    b.Property<byte>("RfAddress");

                    b.Property<long>("Signature");

                    b.HasKey("Id");

                    b.HasIndex("RfAddress")
                        .IsUnique();

                    b.ToTable("Node");
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.NodeMetadata", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .HasColumnName("Name")
                        .HasMaxLength(50);

                    b.Property<string>("Version")
                        .HasColumnName("Version")
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.ToTable("Node");
                });

            modelBuilder.Entity("HelloHome.Central.Domain.Entities.NodeMetadata", b =>
                {
                    b.HasOne("HelloHome.Central.Domain.Entities.Node", "Node")
                        .WithOne("Metadata")
                        .HasForeignKey("HelloHome.Central.Domain.Entities.NodeMetadata", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
