using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using IoTTestbed.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IoTTestbed.Pages.TaskList
{
    public class AuthenticateModel : PageModel
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _db;


        [BindProperty]
        public User user { get; set; }

        public AuthenticateModel(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }


        public void OnGet()
        {
        }


        //public async Task<IActionResult> onPostAuthenticate()
        //{

        //    var user = await _db.User.FirstOrDefaultAsync(o => o.Email == Email);

        //    if (user != null)
        //    {

        //        var passwordHasher = new PasswordHasher<string>();
        //        if (passwordHasher.VerifyHashedPassword(null, user.Password, Password) == PasswordVerificationResult.Success)
        //        {
        //            var claims = new List<Claim> { new Claim(ClaimTypes.Name, Email) };
        //            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        //            return RedirectToPage("/TaskList/Index");
        //        }
        //    }
        //    return Page();
        //}



        public void OnPostRequestAccount()
        {
            
           
            var passwordHasher = new PasswordHasher<string>();
            Debug.Print(passwordHasher.HashPassword(null, this.user.Password));
            // var user = new User() {Email=this.Email, Password=this.Password, Institution=this.Institution };


            //await _db.User.AddAsync(user);
            //await _db.SaveChangesAsync();

           

        }


    }
}
