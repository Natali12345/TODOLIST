using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class UserController : Controller
    {
        private readonly DBContext _context;
        public UserController(DBContext c)
        {
            
            _context = c;

        }
        public IActionResult ShowLogin()
        {
            return View();
        }
        public IActionResult ShowRegistration()
        {
        
            return View();

        }


        [HttpPost]
        public IActionResult CreateRegistration(RegistrationModel model)
        {
           

            if (model.password == model.password2)
            {

                _context.userInfo.Add(new UserInfo()
                {
                    
                    LoginUser = model.email,
                    PasswordUser = model.password
                });
           
                _context.SaveChanges();

                return RedirectToAction("ShowLogin");
            }
            else
            {
                TempData["LoginError"] = "passwords do not match";
                return RedirectToAction("ShowRegistration");

            }
        }

        public async Task<IActionResult> GoToLogin(LoginModel model)
        {

            if (_context.userInfo.Any(z => z.LoginUser == model.email && z.PasswordUser == model.password))
            {

                var claims = new Claim[]
                {
                    new Claim("sub", model.email),
                    new Claim(ClaimTypes.Name, model.email)
                };


                await HttpContext.SignInAsync(
                    new System.Security.Claims.ClaimsPrincipal(new ClaimsIdentity(
                         claims, CookieAuthenticationDefaults.AuthenticationScheme)), new AuthenticationProperties()
                         {

                         });


                return RedirectToAction("ListToday", "List");
            }
            else
            {
                TempData["Error"] = "no correct password or email";
                return RedirectToAction("ShowLogin");


            }
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
