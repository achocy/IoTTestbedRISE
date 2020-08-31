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
namespace IoTTestbed.Pages.TaskList
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public BufferedSingleFileUploadPhysical FileUpload { get; set; }
        private readonly long _fileSizeLimit;
        private readonly string[] _permittedExtensions = { ".txt" };
        private readonly string _targetFilePath= @"C:\Users\Andreas\source\repos\IoTTestbed\IoTTestbed\Files\";
        public string Result { get; private set; }



        private readonly ApplicationDbContext _db;

        public CreateModel(ApplicationDbContext db)

        {

            _db = db;

        }

        public IEnumerable<Sensor> Sensors { get; set; }
        public IEnumerable<Sensor> AvailableSensors { get; set; }

        private string testFile;
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