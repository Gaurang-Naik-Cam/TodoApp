using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Todo.Models;
using Todo.Models.ViewModels;
using Todo.Data;
using Microsoft.EntityFrameworkCore;

namespace Todo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TodoContext _context;

        public HomeController(ILogger<HomeController> logger, TodoContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
              var todoListViewModel = GetAllTodosEF();
             return View(todoListViewModel);
           // return View(await _context.TodoItems.ToListAsync());
        }

        [HttpGet]
        public JsonResult PopulateForm(int id)
        {
            var todo = GetById(id);
            return Json(todo);
        }


        internal TodoViewModel GetAllTodos()
        {
            List<TodoItem> todoList = new();

            using (SqliteConnection con =
                   new SqliteConnection("Data Source=db.sqlite"))
            {
                using (var tableCmd = con.CreateCommand())
                {
                    con.Open();
                    tableCmd.CommandText = "SELECT * FROM todo";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                todoList.Add(
                                    new TodoItem
                                    {
                                        Id = reader.GetInt32(0),
                                        Name = reader.GetString(1)
                                    });
                            }
                        }
                        else
                        {
                            return new TodoViewModel
                            {
                                TodoList = todoList
                            };
                        }
                    };
                }
            }

            return new TodoViewModel
            {
                TodoList = todoList
            };
        }

        internal TodoViewModel GetAllTodosEF()
        {
            List<TodoItem> todoList = _context.TodoItems.ToList();

            return new TodoViewModel
            {
                TodoList = todoList
            };
        }


        internal TodoItem GetById(int id)
        {
            TodoItem todo = _context.TodoItems.ToList().Where(i => i.Id==id).FirstOrDefault();


            //using (var connection =
            //       new SqliteConnection("Data Source=db.sqlite"))
            //{
            //    using (var tableCmd = connection.CreateCommand())
            //    {
            //        connection.Open();
            //        tableCmd.CommandText = $"SELECT * FROM todo Where Id = '{id}'";

            //        using (var reader = tableCmd.ExecuteReader())
            //        {
            //            if (reader.HasRows)
            //            {
            //                reader.Read();
            //                todo.Id = reader.GetInt32(0);
            //                todo.Name = reader.GetString(1);
            //            }
            //            else
            //            {
            //                return todo;
            //            }
            //        };
            //    }
            //}

            return todo;
        }

        // public RedirectResult Insert(TodoItem todo)
        public JsonResult Insert(TodoItem todo)
        {
            _context.TodoItems.Add(todo);
            _context.SaveChanges();
            //using (SqliteConnection con =
            //       new SqliteConnection("Data Source=db.sqlite"))
            //{
            //    using (var tableCmd = con.CreateCommand())
            //    {
            //        con.Open();
            //        tableCmd.CommandText = $"INSERT INTO todo (name) VALUES ('{todo.Name}')";
            //        try
            //        {
            //            tableCmd.ExecuteNonQuery();
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine(ex.Message);
            //        }
            //    }
            //}
            return Json(new { });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
           var item = _context.TodoItems.ToList().Where(i => i.Id == id).FirstOrDefault();
            if(item != null)
            {
                _context.TodoItems.Remove(item);
                _context.SaveChanges();
            }

            //using (SqliteConnection con =
            //       new SqliteConnection("Data Source=db.sqlite"))
            //{
            //    using (var tableCmd = con.CreateCommand())
            //    {
            //        con.Open();
            //        tableCmd.CommandText = $"DELETE from todo WHERE Id = '{id}'";
            //        tableCmd.ExecuteNonQuery();
            //    }
            //}

            return Json(new {});
        }

        public JsonResult Update(TodoItem todo)
        {
            _context.TodoItems.Update(todo);
            _context.SaveChanges();

            //using (SqliteConnection con =
            //       new SqliteConnection("Data Source=db.sqlite"))
            //{
            //    using (var tableCmd = con.CreateCommand())
            //    {
            //        con.Open();
            //        tableCmd.CommandText = $"UPDATE todo SET name = '{todo.Name}' WHERE Id = '{todo.Id}'";
            //        try
            //        {
            //            tableCmd.ExecuteNonQuery();
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine(ex.Message);
            //        }
            //    }
            //}

            //  return Redirect("https://localhost:5001/");
            return Json(new { });
        }

        //public RedirectResult UpdateEF(TodoItem todo)
        //{

        //}
    }
}
