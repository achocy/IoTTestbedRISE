using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IoTTestbed.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IoTTestbed.Pages.Admin
{
    public class ExperimentsModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public string filter { get; set; }
        public IEnumerable<Experiment> Experiments { get; set; }


        public IEnumerable<SensorExperiment> sensexp { get; set; }

        public ExperimentsModel(ApplicationDbContext db)

        {
            _db = db;
        }
        public void OnGet()
        {

            Experiments = _db.Experiment.ToList();

            

        }


        public void  OnGetFilterRunning() {

            Experiments =  _db.Experiment.Where(o => o.Status == "running").ToList();
            Experiments.Reverse();

            //return RedirectToPage()
        }


        public void OnGetFilterFinished()
        {

            Experiments = _db.Experiment.Where(o => o.Status == "finished").ToList();
            Experiments.Reverse();

            //return RedirectToPage()
        }

        public void OnGetFilterScheduled()
        {

            Experiments = _db.Experiment.Where(o => o.Status == "ready").ToList();
            Experiments.Reverse();

            //return RedirectToPage()
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

            return RedirectToPage();
        }





    }
}
