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
    public class ExistingModel : PageModel
    {

        [BindProperty]
        public BufferedSingleFileUploadPhysical FileUpload { get; set; }

        private readonly ILogger<SftpService> logger = new Logger<SftpService>(new NullLoggerFactory());
        private readonly ApplicationDbContext _db;
        private readonly long _fileSizeLimit;
        private readonly string[] _permittedExtensions = { ".txt" };
        private readonly string _targetFilePath = @"C:\Users\Andreas\source\repos\IoTTestbed\IoTTestbed\Files\";

        public IEnumerable<SensorExperiment> SensorExperiments;

        //public IEnumerable<int> _SelectedSensors { get; set; }
        public List<string> ContikiExamples = new List<string>();
        private string testFile;

        [BindProperty]
        public Experiment Experiment { get; set; }



        public ExistingModel(ApplicationDbContext db)
        {
            _db = db;

        }

        public async Task OnGet()
        {


            SensorExperiments = await _db.SensorExperiment.Where(o => o.ExperimentId == 57).ToListAsync();

         

            //Debug.Print("Reachable code");
            var config = new SftpConfig
            {
                Host = "192.168.137.220",
                Port = 22,
                UserName = "pi",
                Password = "1234"
            };
            var sftp = new SftpService(logger, config);

            foreach (var item in sftp.GetDirectories("/home/pi/contiki/examples"))
                ContikiExamples.Add(item.Name);


            ContikiExamples.Remove(".");
            ContikiExamples.Remove("..");
            ContikiExamples.Sort();

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
                Host = "192.168.137.220",
                Port = 22,
                UserName = "pi",
                Password = "1234"
            };

            var sftp = new SftpService(logger, config);

            sftp.CreateDirectory("/home/pi/testdirectory");



            return RedirectToPage("Continue");
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
