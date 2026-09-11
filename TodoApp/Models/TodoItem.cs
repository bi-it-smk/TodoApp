namespace smk_practical_project.Models;

public class TodoItem
{
    public int Id { get;set; }
    public string? Title { get;set; }
    public bool IsCompleted { get; set; } = false;

    public bool IsActive { get; set; } = true;

    public int Priority {get; set; } 
}