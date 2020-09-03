
using IoTTestbed.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoTTestbed.Models

{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Sensor> Sensor { get; set; }
        public DbSet<Experiment> Experiment { get; set; }
        public DbSet<SensorExperiment> SensorExperiment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SensorExperiment>()
                .HasKey(t => new { t.SensorId, t.ExperimentId });

            modelBuilder.Entity<SensorExperiment>()
                .HasOne(pt => pt.Sensor)
                .WithMany(p => p.SensorExperiment)
                .HasForeignKey(pt => pt.SensorId);

            modelBuilder.Entity<SensorExperiment>()
                .HasOne(pt => pt.Experiment)
                .WithMany(t => t.SensorExperiment)
                .HasForeignKey(pt => pt.ExperimentId);
        }



    }
}

