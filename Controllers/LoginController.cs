using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LeaveScheduler.Data;
using LeaveScheduler.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace LeaveScheduler.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly LeaveSchedulerContext _context;
        private string message;

        public LoginController(LeaveSchedulerContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        // GET: Login
        public IActionResult Index()
        {
            return View();
        }

        public User AuthenticateUser(string username, string pwd)
        {
            var result = from p in _context.User
                         where p.UserName == username && p.Password == pwd
                         select p;
            return result.FirstOrDefault();
        }
        [AllowAnonymous]
        public async Task<IActionResult> Login([Bind("UserName,Password")] User input)
        {
            if (ModelState.IsValid)
            {
                // Use Input.Email and Input.Password to authenticate the user
                // with your custom authentication logic.
                //
                // For demonstration purposes, the sample validates the user
                // on the email address maria.rodriguez@contoso.com with 
                // any password that passes model validation.

                User user = AuthenticateUser(input.UserName, input.Password);
                Console.WriteLine(input.UserName + " PASSWORD?????" + input.Password);
                ViewData["message"] = message;
                if (user == null)
                {
                    message = "Invalid login attempt.";
                    return LocalRedirect($"~/Login");
                }

                var claims = new List<Claim>
                {
                    new Claim("Name", user.UserName),
                    new Claim("EmployeeID", user.EmployeeID.ToString()),
                };


                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    // Refreshing the authentication session should be allowed.

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                //redirect to schedule list after authentication
                return LocalRedirect($"~/Schedule");
            }

            // Something failed. Redisplay the form.
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            // Clear the existing external cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return LocalRedirect($"~/Login/");
        }

        private bool UserExists(int id)
        {
          return _context.User.Any(e => e.UserID == id);
        }
    }
}
