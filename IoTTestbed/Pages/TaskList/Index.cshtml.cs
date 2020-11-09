using IoTTestbed.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public IEnumerable<Experiment> Experiments { get; set; }
        public async Task OnGet()




        {

            Experiments = await _db.Experiment.ToListAsync();
            //AvailableSensors = await _db.Sensor.Where(o => o.Status == "Available").ToListAsync();
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
