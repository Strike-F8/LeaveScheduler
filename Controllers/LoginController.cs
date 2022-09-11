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
    public class LoginController : Controller
    {
        private readonly LeaveSchedulerContext _context;

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

        public IActionResult Login([Bind("UserID,EmployeeID,UserName,Password")] User user)
        {
            var cred = from p in _context.User
                        where p.UserName == user.UserName
                        select p.Password;

            if (cred == null)
            {
                Console.WriteLine($"Username {user.UserName} does not match");
                ViewData["message"] = "Login is null";
                return RedirectToAction("Login");
            }

            string pwd = cred.ToString();
            Console.WriteLine("Password is "+pwd);
            if (user.Password != pwd)
            {
                ViewData["message"] = $"Password: {pwd} is incorrect";
                return RedirectToAction("Login");
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.UserName)
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Schedule");
        }

        private bool UserExists(int id)
        {
          return _context.User.Any(e => e.UserID == id);
        }
    }
}
