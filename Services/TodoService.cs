using System.IO;
using System.Text.Json;
using System.Windows.Documents;
using TodoApp.Models;

namespace TodoApp.Services;

public class TodoService
{ 
    // Saves next to your .exe
    private static readonly string FilePath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, "todos.json"
    );

    public static List<Todo> Load()
    {
        if (!File.Exists(FilePath))
            return new List<Todo>();
        
        string json = File.ReadAllText(FilePath);
        List<Todo> todos = JsonSerializer.Deserialize<List<Todo>>(json) ?? new List<Todo>();
        
        return todos.OrderByDescending(t => t.CreatedAt).ToList();
    }

    public static void Save(IEnumerable<Todo> todos)
    {
        string json = JsonSerializer.Serialize(todos, new JsonSerializerOptions { WriteIndented = true });
        
        if (string.IsNullOrWhiteSpace(json)) return;
        
        File.WriteAllText(FilePath, json);
    }
}