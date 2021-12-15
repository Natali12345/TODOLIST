using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Controllers
{
    public class ListController : Controller
    {
        public IActionResult ListToday()
        {
            return View();
        }
        public IActionResult ListCome()
        {
            return View();
        }
    }
}
