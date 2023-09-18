using System;
using System.Collections.Generic;

namespace ToDoList.Models
{
    public class doneTaskModel
    {


        public List<UserTask> Tasks { get; internal set; }
       
    
        public string From { get; set; }

        public string To { get; set; }
        public string SearchText { get; internal set; }
        public string DateTask { get; set; }   
    }
}
