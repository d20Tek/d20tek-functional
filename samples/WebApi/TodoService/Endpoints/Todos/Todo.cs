namespace TodoService.Endpoints.Todos;

public sealed class Todo
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

    public Todo SetId(int id)
    {
        Id = id;
        return this;
    }
}
