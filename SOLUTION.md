# Solution Documentation

**Candidate Name:** Prajakta Gore
**Completion Date:** 05/02/2026

---

## Problems Identified

_Describe the issues you found in the original implementation. Consider aspects like:_
- Architecture and design patterns
- Code quality and maintainability
- Security vulnerabilities
- Performance concerns
- Testing gaps
---------------------

After reviewing the original project, I noticed that the application worked functionally, but the structure was not suitable for production use.

Main issues I found:

- Controller was directly creating `TodoService` using `new`
- No dependency injection
- Service layer contained SQL queries (business logic mixed with database code)
- Tight coupling between components
- Difficult to unit test
- SQL built using string interpolation (risk of SQL injection)
- All endpoints used POST (not RESTful)
- No validation or consistent error responses
- Tests were very limited and depended on the real database

Because of these issues, the code was hard to maintain, hard to test, and not scalable.

---

## Architectural Decisions

_Explain the architecture you chose and why. Consider:_
- Design patterns applied
- Project structure changes
- Technology choices
- Separation of concerns

I refactored the project to follow a layered structure:

Controller → Service → Repository → Database

Responsibilities:

- Controller: handles HTTP requests and responses only
- Service: contains business logic
- Repository: handles database operations
- Model: represents data and validation

### What I introduced

- Dependency Injection
- ITodoService and ITodoRepository interfaces
- Repository pattern to separate SQL from business logic
- DataAnnotations for validation
- RESTful endpoints
- Unit tests using xUnit and Moq

### Why

This separation makes the code:

- easier to read
- easier to test
- easier to change in the future
- loosely coupled

For example, now I can mock the repository in tests without touching the database.


---

## Trade-offs

_Discuss compromises you made and the reasoning behind them. Consider:_
- What did you prioritize?
- What did you defer or simplify?
- What alternatives did you consider?

### Prioritized
- Clean architecture
- Testability
- Security improvements
- Correct REST API design

### Simplified
- Used SQLite and raw SQL instead of EF Core (kept it simple for this exercise)
- No authentication/authorization
- No pagination or filtering

### Alternatives considered
- Using Entity Framework Core
- Using PUT instead of PATCH (chose PATCH for partial updates)

---

## How to Run


### Prerequisites
- .NET 8 SDK
- Git
- 
### Build
```bash
dotnet build
```

### Run
```bash
Run API
dotnet run --project TodoApi

Swagger will be available at:
https://localhost:<port>/swagger
```

### Test
```bash
dotnet test

```

---

## API Documentation

### Endpoints

#### Create TODO
```
Method: POST
URL: /api/todos

Request Body:
{
"title": "Learn .NET",
"description": "Practice building APIs",
"isCompleted": false
}

Response:
Status: 201 Created
{
"message": "Todo created successfully",
"data": {
"id": 1,
"title": "Learn .NET",
"description": "Practice building APIs",
"isCompleted": false,
"createdAt": "2026-02-03T10:30:00Z"
}
}
```

#### Get TODO(s)
```
Method: GET
URL: /api/todos

Request:
No body required

Response:
Status: 200 OK
[
{
"id": 1,
"title": "Learn .NET",
"description": "Practice building APIs",
"isCompleted": false,
"createdAt": "2026-02-03T10:30:00Z"
}
]

Method: GET
URL: /api/todos/1

Request:
No body required

Response:
Status: 200 OK
{
"id": 1,
"title": "Learn .NET",
"description": "Practice building APIs",
"isCompleted": false,
"createdAt": "2026-02-03T10:30:00Z"
}
```

#### Update TODO
```
Method: PATCH
URL: /api/todos/1

Request Body (partial update allowed):
{
"title": "Learn ASP.NET Core"
}

Response:
Status: 200 OK
{
"message": "Todo updated successfully",
"data": {
"id": 1,
"title": "Learn ASP.NET Core",
"description": "Practice building APIs",
"isCompleted": false,
"createdAt": "2026-02-03T10:30:00Z"
}
}
```

#### Delete TODO
```
Method: DELETE
URL: /api/todos/1

Request:
No body required

Response:
Status: 200 OK
{
"message": "Todo deleted successfully"
}
```

---

## Future Improvements

_What would you do if you had more time? Consider:_
- Additional features
- Performance optimizations
- Enhanced testing
- Better documentation
- Deployment considerations

If I had more time, I would:

Replace raw SQL with Entity Framework Core
Add global exception handling middleware
Add logging
Add integration tests
Add pagination and filtering
Use DTOs instead of returning models directly
Add authentication/authorization
