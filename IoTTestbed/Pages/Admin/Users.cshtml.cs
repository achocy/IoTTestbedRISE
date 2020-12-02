using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IoTTestbed.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IoTTestbed.Pages.Admin
{
    public class UsersModel : PageModel
    {


        private readonly ApplicationDbContext _db;

        public IEnumerable<User> users { get; set; }

        public UsersModel(ApplicationDbContext db)
        {
            _db = db;

        }


        public void OnGet()
        {

            users = _db.User.ToList();



        }


        public async Task<IActionResult> OnPostBlock(int uid)
        {


            var user = await _db.User.FindAsync(uid);

            user.Approved = false;
            await _db.SaveChangesAsync();


            return RedirectToPage();
        }


    }
}
