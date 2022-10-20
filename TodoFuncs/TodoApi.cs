
using TodoFuncs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TodoFuncs.Data;

namespace TodoFuncs
{

    public class TodoApi
    {
        private readonly TodoDbContext _db;

        public TodoApi(TodoDbContext db)
        {
            _db = db;
        }
        
       // private static List<Todo> items = new List<Todo>();
        [FunctionName("CreateTodoFunction")]
        public async Task<IActionResult> CreateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todo")]
            HttpRequest req, ILogger log
        )
        {
            log.LogInformation("Creating new todo list item");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<TodoCreateModel>(requestBody);

            var todo = new Todo()
            {
                TaskDescription = input.TaskDescription
            };

            _db.Todos.Add(todo);
            await _db.SaveChangesAsync();
            return new OkObjectResult(todo);
        }

        [FunctionName("GetTodos")]
        public IActionResult GetTodos([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo")] HttpRequest req, ILogger log)
        {
            log.LogInformation("Getting todo items list");

            List<Todo> todos = _db.Todos.ToList();
            //return new OkObjectResult(items);
            return new OkObjectResult(todos);
        }

        [FunctionName("GetTodoById")]
        public IActionResult GetTodoByIdAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo/{id}")] HttpRequest req, ILogger log, string id)
        {
            //var todoItem = items.FirstOrDefault(x => x.Id == id);
            var todo = _db.Todos.FirstOrDefaultAsync(x => x.Id == id);

            if (todo == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(todo);
        }

        [FunctionName("UpdateTodo")]
        public async Task<IActionResult> UpdateTodo([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "todo/{id}")] HttpRequest req, ILogger log, string id)
        {
            var todo =await _db.Todos.FirstOrDefaultAsync(x => x.Id == id);
            if (todo == null)
            {
                return new NotFoundResult();
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<TodoUpdateModel>(requestBody);

            todo.IsCompleted = updated.IsCompleted;

            if (!string.IsNullOrEmpty(updated.TaskDescription))
            {
                todo.TaskDescription = updated.TaskDescription;
            }

            await _db.SaveChangesAsync();
            return new OkObjectResult(todo);

        }
        [FunctionName("DeleteTodo")]
        public async Task<IActionResult> DeleteTodo([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todo/{id}")] HttpRequest req, ILogger log, string id)
        {
            //var todo = items.FirstOrDefault(x => x.Id == id);
            var todo =await _db.Todos.FirstOrDefaultAsync(x => x.Id == id);
            if (todo == null)
                return new NotFoundResult();

            _db.Todos.Remove(todo);
            await _db.SaveChangesAsync();
            return new OkObjectResult($"Todo with id: {id} has been deleted");
        }
    }


}


