using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repo;

        public TodoService(ITodoRepository repo)
        {
            _repo = repo;
        }

        public Todo CreateTodo(Todo todo) => _repo.Create(todo);
        public List<Todo> GetAllTodos() => _repo.GetAll();
        public Todo? GetTodoById(int id) => _repo.GetById(id);
        public Todo UpdateTodo(int id, Todo todo) => _repo.Update(id, todo);
        public bool DeleteTodo(int id) => _repo.Delete(id);
    }
}
