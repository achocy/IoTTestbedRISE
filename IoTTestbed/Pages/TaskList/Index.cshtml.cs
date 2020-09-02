using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IoTTestbed.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IoTTestbed.Pages.TaskList
{
    public class IndexModel : PageModel
    {

        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)

        {

            _db = db;

        }

        public IEnumerable<Experiment> Experiments { get; set; }


        public async Task OnGet()
        {

            Experiments = await _db.Experiment.ToListAsync();

            //AvailableSensors = await _db.Sensor.Where(o => o.Status == "Available").ToListAsync();


        }


    }
}