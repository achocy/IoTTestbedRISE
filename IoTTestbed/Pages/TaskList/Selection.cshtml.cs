using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IoTTestbed.SFTPService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;


using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;

using System.Threading.Tasks;
using IoTTestbed.Models;

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
    public class SelectionModel : PageModel
    {

        [BindProperty]
        public BufferedSingleFileUploadPhysical FileUpload { get; set; }

        private readonly ApplicationDbContext _db;
        public SelectionModel(ApplicationDbContext db)
        {
            _db = db;

        }


        private readonly ILogger<SftpService> logger = new Logger<SftpService>(new NullLoggerFactory());

        private readonly long _fileSizeLimit;
        private readonly string[] _permittedExtensions = { ".txt" };
        private readonly string _targetFilePath = @"C:\Users\Andreas\source\repos\IoTTestbed\IoTTestbed\Files\";

        [BindProperty]
        public string NewProject { get; set; }
        [BindProperty]
        public IEnumerable<SelectedSensor> SelectedSensors { get; set; }
        public Sensor sel;
        public Experiment Experiment { get; set; }

        [BindProperty]
        public IEnumerable<int> SelectedSensorsIDs { get; set; }

        public async Task OnGet(int ExperimentId)
        {


            Experiment = _db.Experiment.First(o => o.ExperimentId == ExperimentId);


            Debug.Print(ExperimentId.ToString());

            var SelectedSensorsv = _db.SensorExperiment.Where(o => o.ExperimentId == ExperimentId).Join(
                 _db.Sensor,
                 Sensor => Sensor.SensorId,
                 SensorExperiment => SensorExperiment.SensorId,
                 (Sensor, SensorExperiment) => new SelectedSensor
                 {
                     SensorId = Sensor.SensorId,
                     Risp = Sensor.Sensor.RasIp,
                     Status = Sensor.Sensor.Status


                 }).ToList();


            SelectedSensors = SelectedSensorsv;

            //  SensorExperiments = await _db.SensorExperiment.Where(o => o.ExperimentId == 57).ToListAsync();


        }
        public void OnPostCreateNew()
        {


            Debug.Print("Reachable code");
            foreach (int SelectedSensorId in SelectedSensorsIDs)
            {

                var SelectedSensor = _db.Sensor.First(o => o.SensorId == SelectedSensorId);
                sel = SelectedSensor;
                var config = new SftpConfig
                {
                    Host = SelectedSensor.RasIp,
                    Port = 22,
                    UserName = "pi",
                    Password = "1234"
                };

                var sftp = new SftpService(logger, config);


                sftp.CreateDirectory("/home/pi/test/" + NewProject);

            }


        }



        public async Task<IActionResult> OnPostUploadAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    Result = "Please correct the form.";

            //    return Page();
            //}

            var formFileContent =
                await FileHelpers.ProcessFormFile<BufferedSingleFileUploadPhysical>(
                    FileUpload.FormFile, ModelState, _permittedExtensions,
                    _fileSizeLimit);


            Debug.Print("Comes in here");
            // For the file name of the uploaded file stored
            // server-side, use Path.GetRandomFileName to generate a safe
            // random file name.
            var trustedFileNameForFileStorage = Path.GetRandomFileName();
            Debug.Print(trustedFileNameForFileStorage);
            var filePath = Path.Combine(
                _targetFilePath, trustedFileNameForFileStorage);


            using (var fileStream = System.IO.File.Create(filePath))
            {

                await FileUpload.FormFile.CopyToAsync(fileStream);
            }

            var config = new SftpConfig
            {
                Host = sel.RasIp,
                Port = 22,
                UserName = "pi",
                Password = "1234"
            };

            var sftp = new SftpService(logger, config);
            sftp.UploadFile(_targetFilePath + FileUpload.FormFile.FileName, "/home/pi/" + NewProject);


            return RedirectToPage();
        }




    }




    public class BufferedSingleFileUploadPhysical
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }

        [Display(Name = "Note")]
        [StringLength(50, MinimumLength = 0)]
        public string Note { get; set; }
    }


}


public class SelectedSensor
{

    public int SensorId;
    public string Risp;
    public string Status;

}

