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

    }
}
