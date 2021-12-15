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
            //so you do undo all the time when try stuff, but f you know you will just try, you can mae sure you 
            //check in the file first like this.
            //this show what different in files.
            //can compare history of stuff

          // why this use need? can be hard to get back to what WAS if do alot changes. maybe forget what was good.
          // so this come back all? how this work? 
          //this like undo but for whole file back to old. the last time you told git to commit change.
          //this like sql transaction. you commit code when it good.
          //you can undo all changes when bad.

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
