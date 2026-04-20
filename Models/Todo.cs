namespace TodoApp.Models;

public class Todo
{
    public string Title { get; set; }
    // public string Description { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}