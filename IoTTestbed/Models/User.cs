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
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Usage { get; set; }
        
        public bool Approved { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Institution { get; set; }
        public string Role { get; set; } = "user";
        public ICollection<Experiment> Experiments { get; set; }

    }
}
