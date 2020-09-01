using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IoTTestbed.Models
{
    public class Sensor
    {
        [Key]
        public int SensorId { get; set; }

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
        public int ExperimentId { get; set; }
        [Required]
        public string ExperimentName { get; set; }
        public string Info { get; set; }

        public ICollection<SensorExperiment> SensorExperiment { get; set; }

    }



    public class SensorExperiment
    {

        public int SensorId { get; set; }
        public Sensor Sensor { get; set; }

        public int ExperimentId { get; set; }
        public Experiment Experiment { get; set; }


    }


}
