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
    public class ContinueModel : PageModel
    {

        [BindProperty]
        public BufferedSingleFileUploadPhysical FileUpload { get; set; }
 private readonly ApplicationDbContext _db;
        private readonly ILogger<SftpService> logger = new Logger<SftpService>(new NullLoggerFactory());
       
        private readonly long _fileSizeLimit;
        private readonly string[] _permittedExtensions = { ".txt" };
        private readonly string _targetFilePath = @"C:\Users\Andreas\source\repos\IoTTestbed\IoTTestbed\Files\";

        public IEnumerable<SensorExperiment> SensorExperiments;

        //public IEnumerable<int> _SelectedSensors { get; set; }
        public List<string> ContikiExamplesSearch = new List<string>();
        public List<string> ContikiExamples = new List<string>();

        //[BindProperty]
        //public string FilterSearch { get; set; }

        private string testFile;


    


        public ContinueModel(ApplicationDbContext db)
        {
            _db = db;

        }

        public async Task OnGet(string FilterSearch)
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


            if (!String.IsNullOrEmpty(FilterSearch))
            {


                ContikiExamples = ContikiExamples.Where(s => s.Contains(FilterSearch)).ToList();



            }





        }


        public async Task<IActionResult> OnPostUploadAsync()
        {
          
            var formFileContent =
                await FileHelpers.ProcessFormFile<BufferedSingleFileUploadPhysical>(
                    FileUpload.FormFile, ModelState, _permittedExtensions,
                    _fileSizeLimit);
            Debug.Print("Comes in here");
   
            var trustedFileNameForFileStorage = Path.GetRandomFileName();
            Debug.Print(trustedFileNameForFileStorage);
            var filePath = Path.Combine(
                _targetFilePath, trustedFileNameForFileStorage);

            using (var fileStream = System.IO.File.Create(filePath))
            {

                await FileUpload.FormFile.CopyToAsync(fileStream);
            }

            return RedirectToPage();
        }



   

    }


    //public class BufferedSingleFileUploadPhysical
    //{
    //    [Required]
    //    [Display(Name = "File")]
    //    public IFormFile FormFile { get; set; }

    //    [Display(Name = "Note")]
    //    [StringLength(50, MinimumLength = 0)]
    //    public string Note { get; set; }
    //}


}
