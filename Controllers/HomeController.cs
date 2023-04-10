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
            _logger.LogInformation("Fetching data from database");
              var todoListViewModel = GetAllTodos();
             return View(todoListViewModel);
        }

        [HttpGet]
        public JsonResult PopulateForm(int id)
        {
            _logger.LogInformation("Getting todo list by Id");
            var todo = GetById(id);
            return Json(todo);
        }

        internal TodoViewModel GetAllTodos()
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
            return todo;
        }

        public ActionResult Insert(TodoItem todo)
        {
            if (!string.IsNullOrEmpty(todo.Name))
            {
                _context.TodoItems.Add(todo);
                _context.SaveChanges();
                _logger.LogInformation(string.Format("New Todo Item - {0} is added", todo.Name));
            }
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
           var item = _context.TodoItems.ToList().Where(i => i.Id == id).FirstOrDefault();
            if(item != null)
            {
                _context.TodoItems.Remove(item);
                _context.SaveChanges();
                _logger.LogInformation(string.Format("{0} Todo Item is removed.",item.Name));
            }
            return Json(new {});
        }

        [HttpPost]
        public JsonResult ToggleStatus(int id)
        {
            var item = _context.TodoItems.ToList().Where(i => i.Id == id).FirstOrDefault();
            if (item != null)
            {
                item.IsComplete = !item.IsComplete;
                _context.TodoItems.Update(item);
                _context.SaveChanges();
                _logger.LogInformation(string.Format("{0} Todo Item staus is changed to {1}", item.Name, item.IsComplete.ToString()));
            }
            return Json(new { });
        }

        [HttpPost]
        public ActionResult Update(TodoItem todo)
        {
            var item = _context.TodoItems.ToList().Where(i => i.Id == todo.Id).FirstOrDefault();
            item.Name = todo.Name;
            _context.TodoItems.Update(item);
            _context.SaveChanges();
            _logger.LogInformation(string.Format("{0} Todo Item is updated.", todo.Name));
            return RedirectToAction("Index", "Home");
        }

    }
}
