using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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
                byte[] salt = new byte[128 / 8];

                using (RandomNumberGenerator rnd = RandomNumberGenerator.Create())
                    rnd.GetBytes(salt);

                
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: model.password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));
                hashed += "~";
                hashed += Convert.ToBase64String(salt);
                _context.userInfo.Add(new UserInfo()
                {
                    
                    LoginUser = model.email,
                    PasswordUser = hashed 
                    
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
            var user = _context.userInfo.FirstOrDefault(z => z.LoginUser == model.email);
            if(user == null)
            {
                TempData["Error"] = "no correct password or email";
                return RedirectToAction("ShowLogin");
               
            }

            string[] records = user.PasswordUser.Split('~');
            byte[] salt = Convert.FromBase64String(records[1]);
            string passwordHash = records[0];

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: model.password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            if (hashed == passwordHash)
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
