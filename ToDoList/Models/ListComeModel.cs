using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class ListComeModel
    {
        public string Date { get; internal set; }
        public string MonthDate { get; internal set; }
        public List<DaysItem> Days { get; internal set; }     
        public Dictionary<DaysItem, List<UserTask>> Tasks { get; internal set; } = new();
        public string ActionName { get; set; }
    }

    public record DaysItem(string Text, string Link, DateTime Date); // new c# 5 stuff, like class but adds good stuff for == magically
    
}
