﻿// <auto-generated />
using System;
using IoTTestbed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IoTTestbed.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<DateTime>("DateFinished")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateInitialized")
                        .HasColumnType("datetime2");

                    b.Property<double>("Duration")
                        .HasColumnType("float");

                    b.Property<string>("ExperimentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Info")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Log")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Precedence")
                        .HasColumnType("bit");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ExperimentId");

                    b.HasIndex("UserId");

                    b.ToTable("Experiment");
                });

            modelBuilder.Entity("IoTTestbed.Models.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ProjectName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProjectId");

                    b.ToTable("Project");
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

                    b.Property<string>("Filename")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsFileUpload")
                        .HasColumnType("bit");

                    b.Property<bool>("IsProjectCreated")
                        .HasColumnType("bit");

                    b.Property<string>("ProjectName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SensorId", "ExperimentId");

                    b.HasIndex("ExperimentId");

                    b.ToTable("SensorExperiment");
                });

            modelBuilder.Entity("IoTTestbed.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Approved")
                        .HasColumnType("bit");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Institution")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Usage")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("IoTTestbed.Models.Experiment", b =>
                {
                    b.HasOne("IoTTestbed.Models.User", "User")
                        .WithMany("Experiments")
                        .HasForeignKey("UserId");
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
