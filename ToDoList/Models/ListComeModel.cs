using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class ListComeModel
    {
        public string Date { get; internal set; }
        public string MonthDate { get; internal set; }
        public List<DateTime> Days { get; internal set; }     
        public Dictionary<DateTime, List<UserTask>> Tasks { get; internal set; } = new();
        public string ActionName { get; set; }

        public string DayToURI(IUrlHelper urlHelper,DateTime day)
        {
            return urlHelper.Action("ListCome",
            new { dateString = day.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) });
        }

        public   string DateForList(DateTime day)
        {
            return day.ToString("dd MMM · dddd", CultureInfo.InvariantCulture);
        }
        public   string DateToMonthDate(DateTime day)
        {
            return day.ToString("dd", CultureInfo.InvariantCulture);
        }
        public   string DateToDayName(DateTime day)
        {
            return day.ToString("ddd", CultureInfo.InvariantCulture);
        }
    }


}
