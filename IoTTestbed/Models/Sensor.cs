﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IoTTestbed.Models
{
    public class Sensor
    {
        [Key]
        [Display(Name = "ID")]
        public int SensorId { get; set; }

        [Display(Name = "PI IP Address")]
        public string RasIp { get; set; }
        public Boolean Availability { get; set; }

        public string Rime { get; set; }
        [Required]
        public string Status { get; set; }

        public ICollection<SensorExperiment> SensorExperiment { get; set; }

    }

    public class Experiment
    {
        [Key]
        [Display(Name = "ID")]
        public int ExperimentId { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string ExperimentName { get; set; }
        public string Info { get; set; }
        public string Status { get; set; }

        public double Duration { get; set; }

        public string Log { get; set; }

        public bool Precedence { get; set; }

        public DateTime DateInitialized { get; set; }
        public DateTime DateFinished { get; set; }


        [ForeignKey("User")]
        public int? UserId { get; set; }
        public User User { get; set; }
        public ICollection<SensorExperiment> SensorExperiment { get; set; }



    }



    public class SensorExperiment
    {
        public int SensorId { get; set; }
        public Sensor Sensor { get; set; }
        public int ExperimentId { get; set; }
        public Experiment Experiment { get; set; }
        public string ProjectName { get; set; }
        public string Filename { get; set; }
        public Boolean IsFileUpload { get; set; }
        public Boolean IsProjectCreated { get; set; }

    }


}
