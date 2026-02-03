using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/todos")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _service;

        public TodoController(ITodoService service)
        {
            _service = service;
        }

        // ===============================
        // CREATE
        // POST: api/todos
        // ===============================
        [HttpPost]
        public IActionResult Create([FromBody] Todo todo)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    message = "Validation failed",
                    errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                });

            var created = _service.CreateTodo(todo);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                new
                {
                    message = "Todo created successfully",
                    data = created
                });
        }

        // ===============================
        // GET ALL
        // GET: api/todos
        // ===============================
        [HttpGet]
        public IActionResult GetAll()
        {
            var todos = _service.GetAllTodos();
            return Ok(todos);
        }

        // ===============================
        // GET BY ID
        // GET: api/todos/5
        // ===============================
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var todo = _service.GetTodoById(id);

            if (todo == null)
                return NotFound();

            return Ok(todo);
        }

        // ===============================
        // UPDATE
        // Patch: api/todos/5
        // ===============================
        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] Todo todo)
        {
            var existing = _service.GetTodoById(id);

            if (existing == null)
                return NotFound(new { message = "Todo not found" });

            // merge
            if (!string.IsNullOrWhiteSpace(todo.Title))
                existing.Title = todo.Title;

            if (!string.IsNullOrWhiteSpace(todo.Description))
                existing.Description = todo.Description;

            existing.IsCompleted = todo.IsCompleted;

            // ✅ use MODEL VALIDATION here
            var context = new ValidationContext(existing);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(existing, context, results, true))
            {
                return BadRequest(new
                {
                    message = "Validation failed",
                    errors = results.Select(r => r.ErrorMessage)
                });
            }

            var updated = _service.UpdateTodo(id, existing);

            return Ok(new
            {
                message = "Todo updated successfully",
                data = updated
            });
        }



        // ===============================
        // DELETE
        // DELETE: api/todos/5
        // ===============================
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _service.DeleteTodo(id);

            if (!deleted)
                return NotFound(new
                {
                    message = "Todo not found"
                });

            return Ok(new
            {
                message = "Todo deleted successfully"
            });
        }

    }
}
