using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyNotes.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MyNotes.Controllers
{
    public class Account : Controller
    {

        private MyNotesDbContext db;

        public Account(MyNotesDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user.Id);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid login or password");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    User new_user = new User { Email = model.Email, Password = model.Password };
                    db.Users.Add(new_user);
                    await db.SaveChangesAsync();

                    await Authenticate(new_user.Id);

                    return RedirectToAction("Index", "Home");
                } else
                {
                    ModelState.AddModelError("", "Email is already registered");
                }
            } else
            {
                ModelState.AddModelError("", "Invalid email or password");
            }
            return View(model);
        }

        private async Task Authenticate(int userId)
        {
            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userId.ToString())
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {

            var model = await db.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(User.Identity.Name));

            if (model == null)
            {
                return NotFound();
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Profile(User model)
        {

            int id = int.Parse(User.Identity.Name);

            if (id != model.Id)
            {
                return StatusCode(422);
            }
            else
            {

                var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    user.Mobile = model.Mobile;
                    db.Users.Update(user);
                    await db.SaveChangesAsync();
                    return View(user);
                }

            }
        }

    }
}
