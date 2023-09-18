using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList
{
    public class DBContext:DbContext
    {


        public DbSet<UserInfo> userInfo { get; set; }
        public DbSet<UserTask> userTask { get; set; }
        public DBContext(DbContextOptions options) : base(options)
        {
           
        }

  

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer(@"Server=localhost\sqlexpress;Database=UsersInfo;Trusted_Connection=True;");
        }



    }
    public class UserTask
    {
 
        [Key]
        public int Id { get; set; }


        public UserInfo LoginUser { get; set; } 
    
        public string Taskk { get; set; }

        public DateTime TaskCreate { get; set; }

        public DateTime? TaskDone { get; set; }


    }

    public class UserInfo
    {
        [Key]

        public string LoginUser { get; set; }
        public string PasswordUser { get; set; }

    }





}

