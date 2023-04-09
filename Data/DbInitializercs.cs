using Todo.Models;
using System;
using System.Linq;

namespace Todo.Data
{
    public static class DbInitializer
    {
        public static void Init(TodoContext context)
        {
            context.Database.EnsureCreated();

            if (context.TodoItems.Any())
            {
                return;
            }

            var TodoItems = new TodoItem[]
            {
                    new TodoItem{ Name="Groceries", IsComplete = false},
                    new TodoItem{ Name="Assignments", IsComplete = false},
                    new TodoItem{ Name="Driving test", IsComplete = true},
                    new TodoItem{ Name="Sports event", IsComplete = false},

            };
            foreach (TodoItem i in TodoItems)
            {
                context.TodoItems.Add(i);
            }
            context.SaveChanges();

        }
    }
}
