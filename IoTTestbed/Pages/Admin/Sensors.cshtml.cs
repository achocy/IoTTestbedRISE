using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IoTTestbed.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IoTTestbed.Pages
{
    public class SensorsModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public string filter { get; set; }
        public IEnumerable<Sensor> Sensors { get; set; }
        public SensorsModel (ApplicationDbContext db)

        {
            _db = db;
        }
        public void OnGet()
        {

            Sensors = _db.Sensor.ToList();



        }
    }
}
