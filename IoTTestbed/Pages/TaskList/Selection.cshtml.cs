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
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public string NewProject2 { get; set; }
        [BindProperty]
        public IEnumerable<SelectedSensor> SelectedSensors { get; set; }
        public Sensor sel;
        public Experiment Experiment { get; set; }

        [BindProperty]
        public IEnumerable<int> SelectedSensorsIDs { get; set; }

        public Boolean Ready { get; set; }

        [BindProperty]
        public double Duration { get; set; }

        public async Task OnGet(int ExperimentId)
        {

            Experiment = await _db.Experiment.FirstOrDefaultAsync(o => o.ExperimentId == ExperimentId);


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

            SelectedSensorsv.RemoveAll(item => item.Status.Equals("ready"));

            SelectedSensors = SelectedSensorsv;

            if (SelectedSensors.Count() == 0)
                Ready = true;



            //  SensorExperiments = await _db.SensorExperiment.Where(o => o.ExperimentId == 57).ToListAsync();
        }
        public async Task OnPostCreateNew(int ExperimentId)
        {
            Debug.Print("Reachable code");
            foreach (int SelectedSensorId in SelectedSensorsIDs)
            {
                var SelectedSensor = await _db.Sensor.FirstOrDefaultAsync(o => o.SensorId == SelectedSensorId);
                sel = SelectedSensor;
                var config = new SftpConfig
                {
                    Host = SelectedSensor.RasIp,
                    Port = 22,
                    UserName = "pi",
                    Password = "1234"
                };

                var sftp = new SftpService(logger, config);
                sftp.CreateDirectory("/home/pi/contikiFirmwares/experiments/" + ExperimentId);
                sftp.CreateDirectory("/home/pi/contikiFirmwares/experiments/" + ExperimentId + "/" + NewProject);


                var SelectedSensorDb = await _db.SensorExperiment.FirstOrDefaultAsync(o => o.ExperimentId == ExperimentId && o.SensorId == SelectedSensorId);
                SelectedSensorDb.ProjectName = NewProject;
                SelectedSensorDb.IsFileUpload = true;

                SelectedSensor.Status = "ready";

                _db.Sensor.Update(SelectedSensor);
                _db.SensorExperiment.Update(SelectedSensorDb);
            }
            await _db.SaveChangesAsync();
        }



        public async Task<IActionResult> OnPostUploadAsync(int ExperimentId)
        {
            //if (!ModelState.IsValid)
            //{
            //    var Result = "Please correct the form.";
            //    return Page();
            //}
            var CurrentSensorsIDs = await _db.SensorExperiment.Where(o => o.ExperimentId == ExperimentId && o.IsFileUpload == true).Select(o => o.SensorId).ToListAsync();
            var CurrentProjectName = _db.SensorExperiment.FirstOrDefault(o => o.ExperimentId == ExperimentId && o.IsFileUpload == true).ProjectName;
            foreach (int CurrentSensorID in CurrentSensorsIDs)
            {
                var CurrentSensor = await _db.Sensor.FirstOrDefaultAsync(o => o.SensorId == CurrentSensorID);
                var formFileContent =
                    await FileHelpers.ProcessFormFile<BufferedSingleFileUploadPhysical>(
                        FileUpload.FormFile, ModelState, _permittedExtensions,
                        _fileSizeLimit);
                var filename = FileUpload.FormFile.FileName;
                var filePath = Path.Combine(
                    _targetFilePath, filename);
                using (var fileStream = System.IO.File.Create(filePath))
                {
                    await FileUpload.FormFile.CopyToAsync(fileStream);
                }
                var config = new SftpConfig
                {
                    Host = CurrentSensor.RasIp,
                    Port = 22,
                    UserName = "pi",
                    Password = "1234"
                };
                var sftp = new SftpService(logger, config);
                sftp.UploadFile(_targetFilePath + filename, "/home/pi/contikiFirmwares/experiments/" + ExperimentId + "/" + CurrentProjectName + "/" + filename);

                var SelectedSensorDb = await _db.SensorExperiment.FirstOrDefaultAsync(o => o.ExperimentId == ExperimentId && o.SensorId == CurrentSensorID);
                SelectedSensorDb.Filename = filename;
                _db.SensorExperiment.Update(SelectedSensorDb);

            }

            var CurrentExperiment = await _db.Experiment.FirstOrDefaultAsync(o => o.ExperimentId == ExperimentId);
            CurrentExperiment.Status = "ready";
            _db.Experiment.Update(CurrentExperiment);

            await _db.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRunExperiment(int ExperimentId)
        {

            var CurrentSensorsIDs = await _db.SensorExperiment.Where(o => o.ExperimentId == ExperimentId && o.IsFileUpload == true).Select(o => o.SensorId).ToListAsync();
            var CurrentExperiment = await _db.Experiment.FirstOrDefaultAsync(o => o.ExperimentId == ExperimentId);
            CurrentExperiment.Duration = Duration;
            CurrentExperiment.Status = "pending";
            _db.Experiment.Update(CurrentExperiment);
            await _db.SaveChangesAsync();
            foreach (int SensorId in CurrentSensorsIDs)
            {
                var CurrentProjectName = _db.SensorExperiment.FirstOrDefault(o => o.ExperimentId == ExperimentId && o.IsFileUpload == true).ProjectName;
                var CurrentFilename = _db.SensorExperiment.FirstOrDefault(o => o.ExperimentId == ExperimentId && o.IsFileUpload == true).Filename;
                await SFTPService.MQTTClient.ConnectAsync(ExperimentId, Duration);
            }

       


            return RedirectToPage("/TaskList/Create");

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

