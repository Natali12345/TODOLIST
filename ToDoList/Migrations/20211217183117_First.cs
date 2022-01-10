using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoList.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "userInfo",
                columns: table => new
                {
                    LoginUser = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userInfo", x => x.LoginUser);
                });

            migrationBuilder.CreateTable(
                name: "userTask",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginUser1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Taskk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TaskDone = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userTask_userInfo_LoginUser1",
                        column: x => x.LoginUser1,
                        principalTable: "userInfo",
                        principalColumn: "LoginUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_userTask_LoginUser1",
                table: "userTask",
                column: "LoginUser1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userTask");

            migrationBuilder.DropTable(
                name: "userInfo");
        }
    }
}
