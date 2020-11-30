using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IoTTestbed.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IoTTestbed.Pages.Admin
{
    public class RequestsModel : PageModel
    {

        private readonly ApplicationDbContext _db;


        public RequestsModel(ApplicationDbContext db)
        {
            _db = db;

        }

        public IEnumerable<User> UserRequests { get; set; }

        [BindProperty]
        public int UserId { get; set; }



        public void OnGet()
        {


            UserRequests = _db.User.Where(o => o.Approved == false).ToList();



        }


        public async Task<ActionResult> OnPostRequestApprove(int UserId)
        {

            var user = await _db.User.FindAsync(UserId);

            user.Approved = true;

             _db.User.Update(user);
            await _db.SaveChangesAsync();

            return RedirectToPage();
            ;
        }


    }
}
