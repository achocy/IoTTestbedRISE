﻿// <auto-generated />
using IoTTestbed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IoTTestbed.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200903105004_ChangeForeignKeysSensorExperiment")]
    partial class ChangeForeignKeysSensorExperiment
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("IoTTestbed.Models.Experiment", b =>
                {
                    b.Property<int>("ExperimentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ExperimentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Info")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ExperimentId");

                    b.ToTable("Experiment");
                });

            modelBuilder.Entity("IoTTestbed.Models.Sensor", b =>
                {
                    b.Property<int>("SensorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Availability")
                        .HasColumnType("bit");

                    b.Property<string>("RasIp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SensorId");

                    b.ToTable("Sensor");
                });

            modelBuilder.Entity("IoTTestbed.Models.SensorExperiment", b =>
                {
                    b.Property<int>("SensorId")
                        .HasColumnType("int");

                    b.Property<int>("ExperimentId")
                        .HasColumnType("int");

                    b.HasKey("SensorId", "ExperimentId");

                    b.HasIndex("ExperimentId");

                    b.ToTable("SensorExperiment");
                });

            modelBuilder.Entity("IoTTestbed.Models.SensorExperiment", b =>
                {
                    b.HasOne("IoTTestbed.Models.Experiment", "Experiment")
                        .WithMany("SensorExperiment")
                        .HasForeignKey("ExperimentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IoTTestbed.Models.Sensor", "Sensor")
                        .WithMany("SensorExperiment")
                        .HasForeignKey("SensorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
