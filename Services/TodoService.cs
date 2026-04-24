using System.IO;
using System.Text.Json;
using System.Windows.Documents;
using TodoApp.Models;

namespace TodoApp.Services;

public static class TodoService
{ 
    // Saves next to your .exe
    private static readonly string FilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "TodoApp",
        "data.json"
    );
    
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

    public static List<Todo> Load()
    {
        if (!File.Exists(FilePath))
            return new List<Todo>();
        
        string json = File.ReadAllText(FilePath);
        List<Todo> todos = JsonSerializer.Deserialize<List<Todo>>(json, Options) ?? new List<Todo>();
        
        return todos.OrderByDescending(t => t.CreatedAt).ToList();
    }

    public static void Save(IEnumerable<Todo> todos)
    {
        string directory = Path.GetDirectoryName(FilePath)!;
        if (!Directory.Exists(directory)) 
            Directory.CreateDirectory(directory);
        
        string json = JsonSerializer.Serialize(todos, Options);
        File.WriteAllText(FilePath, json);
    }
}