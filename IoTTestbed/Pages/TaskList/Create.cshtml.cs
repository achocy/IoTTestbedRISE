using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IoTTestbed.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IoTTestbed.Pages.TaskList
{
    public class CreateModel : PageModel
    {

        private readonly ApplicationDbContext _db;

        public CreateModel(ApplicationDbContext db)

        {

            _db = db;

        }

        public IEnumerable<Sensor> Sensors { get; set; }
        public IEnumerable<Sensor> AvailableSensors { get; set; }
        public async Task OnGet()
        {

            Sensors = await _db.Sensor.ToListAsync();

            AvailableSensors = await _db.Sensor.Where(o => o.Status == "Available").ToListAsync();


        }
    }
}