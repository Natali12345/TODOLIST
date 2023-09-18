using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ListDoneController : Controller
    {
        private readonly DBContext _context;

        public ListDoneController(DBContext c)
        {
            _context = c;
        }


        [HttpGet]
        public async Task<IActionResult> DoneTaskAsync(string searchTask, string dateStringFrom, string dateStringToo)
        {
            List<UserTask> tasks;
            DateTime? dateFrom = null;
            DateTime? dateTo = null;

           if( DateTime.TryParse(dateStringFrom, System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.AssumeUniversal, out var d))
            {
                dateFrom = d;
            }

           if( DateTime.TryParse(dateStringFrom, System.Globalization.CultureInfo.InvariantCulture,
             System.Globalization.DateTimeStyles.AssumeUniversal, out var d2))
            {
                dateTo = d2;
            }

      
            if (!string.IsNullOrEmpty(searchTask))
            {
                tasks = await _context.userTask.Where(s => s.TaskDone != null
                && s.LoginUser.LoginUser == User.Identity.Name && s.Taskk.Contains(searchTask)
                && (dateFrom == null || s.TaskCreate >= dateFrom)
                && (dateTo == null || s.TaskCreate <= dateTo)
                 ).ToListAsync();

            }  
            
         
            else
            {
                tasks = await _context.userTask.Where(z => z.TaskDone != null
                && z.LoginUser.LoginUser == User.Identity.Name).ToListAsync();
            }
        
            var m = new doneTaskModel()
            {
                SearchText = searchTask,
                Tasks = tasks,
                From = dateFrom.HasValue ? dateFrom.Value.ToString("mm/dd/yyyy") : null,
                To = dateTo.HasValue ? dateTo.Value.ToString("mm/dd/yyyy") : null
            };

            return View(m);
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

            return RedirectToAction("DoneTask" ,
                new { searchTask = Request.Query["searchTask"],
                    dateStringFrom = Request.Query["dateStringFrom"],
                    dateStringTo = Request.Query["dateStringTo"]
                });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeTask(ChangeTaskModel model)// changee this func
        {
            var res = await _context.userTask.FirstOrDefaultAsync(z => z.Id == model.Id);
            if (res != null)
            {

                res.Taskk = model.Text;

                await _context.SaveChangesAsync();
            }
            return RedirectToAction("DoneTask" , new
            {
                searchTask = Request.Query["searchTask"],
                dateStringFrom = Request.Query["dateStringFrom"],
                dateStringTo = Request.Query["dateStringTo"]
            });
        }


        [HttpPost]
        public async Task<IActionResult> ChooseDate(int id,string selectDate)
        {
            //i want you think what wrong in this method
            DateTime.TryParse(selectDate, System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.AssumeUniversal, out var DateChoose);// this good
             
            var res = await _context.userTask.FirstOrDefaultAsync(z => z.Id == id);//this too i think
            if (res != null)
            {
                //
                res.TaskCreate = DateChoose;
                res.TaskDone = null; //now see how work
                await _context.SaveChangesAsync();
            }
            return Ok();// ichange this because this will be json ajax and no need view return
        }
  



    }
}
