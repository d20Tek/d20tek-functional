namespace TodoService.Endpoints.Todos;

public sealed class Todo
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public bool IsCompleted { get; set; }
}
