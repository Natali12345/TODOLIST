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


        public DBContext(DbContextOptions options) : base(options)
        {
           
            Database.EnsureCreated();
        }

  

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer(@"Server=localhost\sqlexpress;Database=UsersInfo;Trusted_Connection=True;");
        }



    }


    public class UserInfo
    {
        [Key]

        public string LoginUser { get; set; }
        public string PasswordUser { get; set; }
    }





}

