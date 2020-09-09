using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IoTTestbed.Models;
using IoTTestbed.SFTPService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using IoTTestbed.utilities;
using Microsoft.Extensions.Configuration;
using Renci.SshNet;
using Microsoft.Extensions.Logging;

namespace IoTTestbed.Pages.TaskList
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public BufferedSingleFileUploadPhysical FileUpload { get; set; }
        private readonly long _fileSizeLimit;
        private readonly string[] _permittedExtensions = { ".txt" };
        private readonly string _targetFilePath = @"C:\Users\Andreas\source\repos\IoTTestbed\IoTTestbed\Files\";
        public string Result { get; private set; }
        List<string> sensorsList = new List<string>();
        private readonly ILogger<SftpService> logger = new Logger<SftpService>(new NullLoggerFactory());
        private readonly ApplicationDbContext _db;
        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Sensor> Sensors { get; set; }
        public IEnumerable<Sensor> AvailableSensors { get; set; }

        [BindProperty]
        public IEnumerable<int> SelectedSensors { get; set; }
        public List<string> ContikiExamples = new List<string>();



        private string testFile = "now";

        [BindProperty]
        public Experiment Experiment { get; set; }
        public SensorExperiment SensorExperiment { get; set; }
        public async Task OnGetAsync()
        {
            Sensors = await _db.Sensor.ToListAsync();
            AvailableSensors = await _db.Sensor.Where(o => o.Status == "Available").ToListAsync();
        }


        public async Task<IActionResult> OnPostContinue()
        {
            await _db.Experiment.AddAsync(Experiment);
            await _db.SaveChangesAsync();

            foreach (int SensorIds in SelectedSensors)
            {
                SensorExperiment se = new SensorExperiment() { SensorId = SensorIds, ExperimentId = Experiment.ExperimentId };
                Debug.Print(se.SensorId.ToString());
                await _db.SensorExperiment.AddAsync(se);
                var result = _db.Sensor.SingleOrDefault(b => b.SensorId == SensorIds);

                if (result != null)
                {
                    result.Status = "active";
                }
            }

            await _db.SaveChangesAsync();

            return RedirectToPage("Selection", new { Experiment = Experiment});
        }





    }

}