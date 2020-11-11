using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace IoTTestbed.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public bool Approved { get; set; }
        public string Role { get; set; }
        public string Institution { get; set; }
        public ICollection<Experiment> Experiments { get; set; }

    }
}
