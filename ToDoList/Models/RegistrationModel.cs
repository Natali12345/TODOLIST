using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class RegistrationModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public string password2 { get; set; }
    }
}
