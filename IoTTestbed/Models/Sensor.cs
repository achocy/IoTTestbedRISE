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
        public int Id { get; set; }

        public string RasIp { get; set; }
        public Boolean Availability { get; set; }

        public string Rime { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
