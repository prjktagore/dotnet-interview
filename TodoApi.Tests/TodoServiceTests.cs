using Moq;
using TodoApi.Models;
using TodoApi.Repositories;
using TodoApi.Services;
using Xunit;

namespace TodoApi.Tests
{
    public class TodoServiceTests
    {
        private readonly Mock<ITodoRepository> _repoMock;
        private readonly ITodoService _service;

        public TodoServiceTests()
        {
            // Create fake repository
            _repoMock = new Mock<ITodoRepository>();

            // Inject fake into service
            _service = new TodoService(_repoMock.Object);
        }

        [Fact]
        public void CreateTodo_Should_Return_Created_Todo()
        {
            // Arrange
            var todo = new Todo { Title = "Test" };

            _repoMock
                .Setup(r => r.Create(It.IsAny<Todo>()))
                .Returns(todo);

            // Act
            var result = _service.CreateTodo(todo);

            // Assert
            Assert.Equal("Test", result.Title);
        }

        [Fact]
        public void GetTodoById_Should_Return_Null_When_NotFound()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetById(1))
                .Returns((Todo?)null);

            // Act
            var result = _service.GetTodoById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void DeleteTodo_Should_Return_True_When_Deleted()
        {
            // Arrange
            _repoMock
                .Setup(r => r.Delete(1))
                .Returns(true);

            // Act
            var result = _service.DeleteTodo(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Delete_Should_Return_False_When_NotFound()
        {
            _repoMock.Setup(r => r.Delete(99)).Returns(false);

            var result = _service.DeleteTodo(99);

            Assert.False(result);
        }

        [Fact]
        public void GetAll_Should_Return_List()
        {
            _repoMock.Setup(r => r.GetAll()).Returns(new List<Todo>
            {
                 new Todo { Title = "Test" }
            });

            var result = _service.GetAllTodos();

            Assert.Single(result);
        }

        [Fact]
        public void CreateTodo_Should_Return_Created_Item()
        {
            var todo = new Todo { Title = "Test" };

            _repoMock.Setup(r => r.Create(It.IsAny<Todo>()))
                     .Returns(todo);

            var result = _service.CreateTodo(todo);

            Assert.NotNull(result);
            Assert.Equal("Test", result.Title);
        }

        [Fact]
        public void GetTodoById_Should_Return_Item_When_Exists()
        {
            var todo = new Todo { Id = 1, Title = "Test" };

            _repoMock.Setup(r => r.GetById(1))
                     .Returns(todo);

            var result = _service.GetTodoById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void GetAllTodos_Should_Return_List()
        {
            var todos = new List<Todo>
            {
                 new Todo { Title = "A" },
                  new Todo { Title = "B" }
            };

            _repoMock.Setup(r => r.GetAll())
                     .Returns(todos);

            var result = _service.GetAllTodos();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void DeleteTodo_Should_Return_False_When_NotFound()
        {
            _repoMock.Setup(r => r.Delete(99))
                     .Returns(false);

            var result = _service.DeleteTodo(99);

            Assert.False(result);
        }

        [Fact]
        public void UpdateTodo_Should_Return_Updated_Item()
        {
            var todo = new Todo { Title = "Updated" };

            _repoMock.Setup(r => r.Update(1, It.IsAny<Todo>()))
                     .Returns(todo);

            var result = _service.UpdateTodo(1, todo);

            Assert.Equal("Updated", result.Title);
        }

    }
}
