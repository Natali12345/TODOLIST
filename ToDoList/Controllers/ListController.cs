using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ListController : Controller
    {

        private readonly DBContext _context;

        public ListController(DBContext c)
        {
            _context = c;
        }


        [HttpGet]
        public async Task<IActionResult> ListToday(bool showCompleted = false)
        {
            List<UserTask> tasks;
            if (showCompleted)
            {
                tasks = await _context.userTask.Where(z => z.TaskDone != null
                && z.LoginUser.LoginUser == User.Identity.Name).ToListAsync();
            }
            else
            {
                tasks = await _context.userTask.Where(z => z.TaskDone == null
                && z.LoginUser.LoginUser == User.Identity.Name).ToListAsync();
            }
            return View(tasks);
        }


        public async Task<IActionResult> ListCome(string dateString = null, bool showCompleted = false)
        {
            DateTime dateTime = DateTime.UtcNow;

            var lc = new ListComeModel();

            if (!string.IsNullOrEmpty(dateString))
            {
                DateTime.TryParse(dateString, System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.AssumeUniversal, out dateTime);
            }

            dateTime = dateTime.Date;

            lc.Date = dateTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            lc.MonthDate = dateTime.ToString("MMMM yyyy", CultureInfo.InvariantCulture);

            List<DaysItem> days = new List<DaysItem>();
            for (var x = 0; x <= 7; x++)
            {
                var day = dateTime.AddDays(x);
                string dayText = DayToDisplay2(day.Date);
                string link = DayToURI(day.Date);
                days.Add(new DaysItem(dayText, link, day.Date));
            }
            lc.Days = days;

            List<UserTask> tasks;
            if (showCompleted)
            {
                tasks = await _context.userTask.Where(z => z.TaskDone != null
                && z.LoginUser.LoginUser == User.Identity.Name
                && z.TaskCreate >= dateTime && z.TaskCreate <= dateTime.AddDays(7)).ToListAsync();
            }
            else
            {
                tasks = await _context.userTask.Where(z => z.TaskDone == null
                && z.LoginUser.LoginUser == User.Identity.Name
                && z.TaskCreate >= dateTime && z.TaskCreate <= dateTime.AddDays(7)).ToListAsync();
            }
            lc.Tasks = tasks.GroupBy(z => z.TaskCreate.Date)
            .ToDictionary(z => new DaysItem(DayToDisplay(z.Key), DayToURI(z.Key), z.Key), z => z.ToList());

            foreach (var d in lc.Days)
            {
                bool find = false;
                foreach(var t in lc.Tasks)
                {
                    if (t.Key.Date == d.Date)
                    {
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                 
                    var td = new DaysItem(DayToDisplay(d.Date), DayToURI(d.Date), d.Date);
                    lc.Tasks.Add(td, new());
                }
               

               
            }
            return View(lc);
        }


        private string DayToURI(DateTime day)
        {
            return Url.Action("ListCome",
            new { dateString = day.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) });
        }

        private static string DayToDisplay(DateTime day)
        {
            return day.ToString("dd MMM - dddd", CultureInfo.InvariantCulture);
        }
        private static string DayToDisplay2(DateTime day)
        {
            return day.ToString("ddd  dd ", CultureInfo.InvariantCulture);
        }

        [HttpPost]
        public async Task<IActionResult> SaveTask(TaskModel model)
        {
            var user = _context.userInfo.First(z => z.LoginUser == User.Identity.Name);
            _context.userTask.Add(new UserTask()
            {
                LoginUser = user,
                Taskk = model.Taskk,
                TaskCreate = model.Create
            });

            await _context.SaveChangesAsync();
            return RedirectToAction(model.ActionName);
        }


        [HttpPost]
        public async Task<IActionResult> CloseTask(int id, ListComeModel model)
        {
            var res = await _context.userTask.FirstOrDefaultAsync(z => z.Id == id);
            if (res != null)
            {
                res.TaskDone = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(model.ActionName);
        }



        [HttpPost]
        public async Task<IActionResult> DeleteTask(int id, ListComeModel model)
        {
            var res = await _context.userTask.FirstOrDefaultAsync(z => z.Id == id);
            if (res != null)
            {

                _context.userTask.Remove(res);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(model.ActionName);
        }


       
    }
}
