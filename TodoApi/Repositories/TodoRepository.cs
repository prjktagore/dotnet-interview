using Microsoft.Data.Sqlite;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly string _connectionString = "Data Source=todos.db";

        public Todo Create(Todo todo)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Todos (Title, Description, IsCompleted, CreatedAt)
                VALUES ($title,$desc,$completed,$created);
                SELECT last_insert_rowid();
            ";

            command.Parameters.AddWithValue("$title", todo.Title);
            command.Parameters.AddWithValue("$desc", todo.Description ?? "");
            command.Parameters.AddWithValue("$completed", todo.IsCompleted ? 1 : 0);
            command.Parameters.AddWithValue("$created", DateTime.UtcNow.ToString("o"));

            todo.Id = Convert.ToInt32(command.ExecuteScalar());
            todo.CreatedAt = DateTime.UtcNow;

            return todo;
        }

        public List<Todo> GetAll()
        {
            var list = new List<Todo>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Todos";

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Todo
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.GetString(2),
                    IsCompleted = reader.GetInt32(3) == 1,
                    CreatedAt = DateTime.Parse(reader.GetString(4))
                });
            }

            return list;
        }

        public Todo? GetById(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Todos WHERE Id=$id";
            command.Parameters.AddWithValue("$id", id);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Todo
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.GetString(2),
                    IsCompleted = reader.GetInt32(3) == 1,
                    CreatedAt = DateTime.Parse(reader.GetString(4))
                };
            }

            return null;
        }

        public Todo Update(int id, Todo todo)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = @"
                UPDATE Todos
                SET Title=$title, Description=$desc, IsCompleted=$completed
                WHERE Id=$id
            ";

            command.Parameters.AddWithValue("$title", todo.Title);
            command.Parameters.AddWithValue("$desc", todo.Description ?? "");
            command.Parameters.AddWithValue("$completed", todo.IsCompleted ? 1 : 0);
            command.Parameters.AddWithValue("$id", id);

            command.ExecuteNonQuery();

            todo.Id = id;
            return todo;
        }

        public bool Delete(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Todos WHERE Id=$id";
            command.Parameters.AddWithValue("$id", id);

            return command.ExecuteNonQuery() > 0;
        }
    }
}
