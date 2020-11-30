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

        public IEnumerable<SensorExperiment> sensexp { get; set; }
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
        public IEnumerable<SensorExperiment> SelectedSensors { get; set; }
        [BindProperty]
        public IList<SelectedSensor> FinalSensors { get; set; }
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

            SelectedSensors = await _db.SensorExperiment.Where(o => o.ExperimentId == ExperimentId && !o.IsFileUpload).ToListAsync(); //needs a join to fetch RIME

            if (SelectedSensors.Count() == 0)
            {
                Ready = true;

                //FinalSensors = _db.SensorExperiment.Where(o => o.ExperimentId == ExperimentId).ToList();
                var FinalSensorsTemp = _db.SensorExperiment.Where(s=>s.ExperimentId == ExperimentId)
          .Join(
              _db.Sensor,
               sensorexp => sensorexp.Sensor.SensorId,
              sensor => sensor.SensorId,  
              (sensorexp,sensor ) => new SelectedSensor
              {
                  SensorId = sensor.SensorId,
                  Rime = sensor.Rime,
                  Filename = sensorexp.Filename,
                  ProjectName = sensorexp.ProjectName,
                  Rsip=sensor.RasIp
              }
          ).ToList();

                FinalSensors = FinalSensorsTemp;
            }

            //  SensorExperiments = await _db.SensorExperiment.Where(o => o.ExperimentId == 57).ToListAsync();
        }
        public async Task OnPostCreateNew(int ExperimentId)
        {
            NewProject = NewProject.Replace(" ", "");
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
                SelectedSensorDb.IsProjectCreated = true;

                //SelectedSensor.Status = "active";

                //_db.Sensor.Update(SelectedSensor);
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
            var CurrentSensorsIDs = await _db.SensorExperiment.Where(o => o.ExperimentId == ExperimentId && o.IsProjectCreated && !o.IsFileUpload).Select(o => o.SensorId).ToListAsync();
            var CurrentProjectName = _db.SensorExperiment.FirstOrDefault(o => o.ExperimentId == ExperimentId && o.IsProjectCreated && !o.IsFileUpload).ProjectName;
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
                SelectedSensorDb.IsFileUpload = true;
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

            await SFTPService.MQTTClient.ConnectAsync(ExperimentId, Duration);

            return RedirectToPage("/TaskList/Index");

        }


        public async Task<IActionResult> OnPostDelete(int ExperimentId, int SensorId)
        {

            System.Diagnostics.Debug.Print("On DeletePost");
            System.Diagnostics.Debug.Print(SensorId.ToString());
            var expSensor = await _db.SensorExperiment.FirstOrDefaultAsync(o => o.ExperimentId == ExperimentId && o.SensorId == SensorId);
            sensexp = _db.SensorExperiment.Where(s => s.ExperimentId == ExperimentId);

            if (sensexp.Count() == 1)
                return RedirectToPage();
            else
                _db.Remove(expSensor);

            await _db.SaveChangesAsync();

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
    public string Rime;
    public string Filename;
    public string ProjectName;
    public string Rsip;

}

