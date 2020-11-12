using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IoTTestbed.Pages.TaskList
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {

            await HttpContext.SignOutAsync();

            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));


            return RedirectToPage("/Index");


        }
    }
}
