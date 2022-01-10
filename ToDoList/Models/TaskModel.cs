using System;

namespace ToDoList.Models
{
    public class TaskModel
    {
        public int Id{ get; set; }

        public string  Login { get; set; }
        public string Taskk { get; set; }
        public DateTime Create { get; set; }
        public DateTime Done { get; set; }
        public string ActionName { get;   set; }
    }
}
