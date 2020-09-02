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
        public IEnumerable<string> SelectedSensors { get; set; }

        private string testFile;

        [BindProperty]
        public Experiment Experiment { get; set; }

        public async Task OnGet()
        {

            Sensors = await _db.Sensor.ToListAsync();

            AvailableSensors = await _db.Sensor.Where(o => o.Status == "Available").ToListAsync();


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



            // **WARNING!**
            // In the following example, the file is saved without
            // scanning the file's contents. In most production
            // scenarios, an anti-virus/anti-malware scanner API
            // is used on the file before making the file available
            // for download or for use by other systems. 
            // For more information, see the topic that accompanies 
            // this sample.

            using (var fileStream = System.IO.File.Create(filePath))
            {
                // await fileStream.WriteAsync(formFileContent);

                // To work directly with a FormFile, use the following
                // instead:
                await FileUpload.FormFile.CopyToAsync(fileStream);
            }

            var config = new SftpConfig
            {
                Host = "192.168.137.28",
                Port = 22,
                UserName = "pi",
                Password = "1234"
            };

            // can be retrieved from appsettings.json
            //using var client = new SftpClient(config.Host, config.Port, config.UserName, config.Password);

            //client.UploadFile(_targetFilePath + trustedFileNameForFileStorage.ToString(), "/home/pi/software/");



            var sftp = new SftpService(logger, config);

            //  sftp.UploadFile(_targetFilePath + trustedFileNameForFileStorage, "/home/pi/software/" + FileUpload.FormFile.FileName);



            //Debug.Print("Here1");
            //Debug.Print(SelectedSensors.ToString());

            

            await _db.Experiment.AddAsync(Experiment);
            await _db.SaveChangesAsync();

            // return RedirectToPage("Index");


            //else
            //  return Page();


            return RedirectToPage("Index");
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