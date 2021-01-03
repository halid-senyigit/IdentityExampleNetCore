using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityExampleNetCore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace IdentityExampleNetCore.Controllers
{
    public class AccountController : Controller
    {
        private readonly ModelContext db;

        public AccountController(ModelContext db)
        {
            this.db = db;
        }


        [Authorize]
        public IActionResult AuthenticatedUsers()
        {
            // TODO: Your code here
            return View();
        }


        [Authorize("admin")]
        public IActionResult OnlyAdministrators()
        {
            // TODO: Your code here
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Name,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Role = "user";
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return Content("model state was not valid");

        }


        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password")] User user)
        {
            User u = db.Users.FirstOrDefault(n => n.Email == user.Email && n.Password == user.Password);
            if (u != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(new[]{
                    new Claim(ClaimTypes.Name, u.Name),
                    new Claim(ClaimTypes.Email, u.Email),
                    new Claim(ClaimTypes.Role, u.Role)
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.error = "email or password is not correct";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }


    }
}