using Apps.Repositories;

namespace TodoService.Endpoints.Todos;

public sealed class Todo : IEntity
{
    public int Id { get; private set; }

    public string Title { get; private set; }
    
    public string? Description { get; private set; }
    
    public bool IsCompleted { get; private set; }

    public Todo(int id, string title, string? description = null, bool isCompleted = false)
    {
        Id = id;
        Title = title;
        Description = description;
        IsCompleted = isCompleted;
    }

    public void SetId(int id) => Id = id;
}
