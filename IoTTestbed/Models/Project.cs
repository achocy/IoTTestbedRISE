using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IoTTestbed.Models
{
    public class Project
    {
        [Key]
        public int ProjectId;

        public int ExperimentId;
    
    }
}
