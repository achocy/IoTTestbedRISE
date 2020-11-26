using IoTTestbed.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace IoTTestbed.Pages.TaskList
{

    public class IndexModel : PageModel
    {

        private readonly ApplicationDbContext _db;

        public IEnumerable<SensorExperiment> sensexp { get; set; }
        public IndexModel(ApplicationDbContext db)

        {
            _db = db;
        }
        [BindProperty]
        public IEnumerable<Experiment> Experiments { get; set; }
        /// int.Parse(HttpContext.Session.GetString("UserId"))



        public async Task<IActionResult> OnGet()

        {

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                //The code
                return RedirectToPage("/TaskList/Authenticate");
            }


            var uid = int.Parse(HttpContext.Session.GetString("UserId"));
            //Experiments = await _db.Experiment.ToListAsync();
            Experiments = _db.Experiment.Where(r => r.UserId == uid).ToList();
            Experiments = Experiments.Reverse();
            //AvailableSensors = await _db.Sensor.Where(o => o.Status == "Available").ToListAsync();

            return Page();

        }
        public async Task<ActionResult> OnPostDownload(int id)
        {
            System.Diagnostics.Debug.Print(id.ToString());

            var exp = await _db.Experiment.FindAsync(id);

            var stream = System.IO.File.OpenRead(@exp.Log);
            System.Diagnostics.Debug.Print("Download");
            return File(stream, "text/plain", "aggregator_log_" + id + ".log");

        }
        public async Task<IActionResult> OnPostDelete(int id)
        {

            System.Diagnostics.Debug.Print("On DeletePost");
            var exp = await _db.Experiment.FindAsync(id);
            if (exp == null)
            {
                return NotFound();
            }


            sensexp = _db.SensorExperiment.Where(s => s.ExperimentId == id);

            foreach (SensorExperiment se in sensexp)
                _db.SensorExperiment.Remove(se);


            _db.Experiment.Remove(exp);
            await _db.SaveChangesAsync();

            return RedirectToPage("Index");
        }





    }
}
