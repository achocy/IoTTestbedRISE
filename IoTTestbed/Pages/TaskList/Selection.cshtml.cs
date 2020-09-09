using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IoTTestbed.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Internal;
using Renci.SshNet;

namespace IoTTestbed.Pages.TaskList
{
    public class SelectionModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public SelectionModel(ApplicationDbContext db)
        {
            _db = db;

        }

        public IEnumerable<int> SelectedSensors { get; set; }


        public async Task OnGet(Experiment Experiment)
        {

            Debug.Print(Experiment.ExperimentId.ToString());

            var SelectedSensorsv = _db.SensorExperiment.Where(o => o.ExperimentId == Experiment.ExperimentId).Join(

                 _db.Sensor,
                 Sensor => Sensor.SensorId,
                 SensorExperiment => SensorExperiment.SensorId,
                 (Sensor, SensorExperiment) => new
                 {
                     SensorId = Sensor.SensorId,
                     Risp = Sensor.Sensor.RasIp,
                     Status = Sensor.Sensor.Status


                 }).ToList();


            Debug.Print(Experiment.ExperimentId.ToString());

            //  SensorExperiments = await _db.SensorExperiment.Where(o => o.ExperimentId == 57).ToListAsync();


        }
    }
}
