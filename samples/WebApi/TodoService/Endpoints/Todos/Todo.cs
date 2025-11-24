namespace TodoService.Endpoints.Todos;

internal sealed class Todo(int id, string title, string? description = null, bool isCompleted = false)
{
    public int Id { get; private set; } = id;

    public string Title { get; private set; } = title;

    public string? Description { get; private set; } = description;

    public bool IsCompleted { get; private set; } = isCompleted;

    public Todo Update(string title, string? description, bool isCompleted)
    {
        Title = title;
        Description = description;
        IsCompleted = isCompleted;

        return this;
    }
}
