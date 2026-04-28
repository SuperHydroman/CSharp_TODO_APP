using System.Diagnostics;
using System.IO;
using System.Text.Json;
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
        try
        {
            if (!File.Exists(FilePath))
            {
                List<Todo> list = new List<Todo>();
                Save(list);
                return list;
            }
            
            string json = File.ReadAllText(FilePath);
            List<Todo> todos = JsonSerializer.Deserialize<List<Todo>>(json, Options) 
                               ?? new List<Todo>();
            
            return todos.OrderByDescending(t => t.CreatedAt).ToList();
        }
        catch (Exception e)
        {
            Debug.WriteLine($"[TodoService] Failed to load: {e.Message}");
            return new List<Todo>();
        }
    }

    public static void Save(IEnumerable<Todo> todos)
    {
        try
        {
            string directory = Path.GetDirectoryName(FilePath)!;
            if (!Directory.Exists(directory)) 
                Directory.CreateDirectory(directory);
        
            string json = JsonSerializer.Serialize(todos, Options);
            File.WriteAllText(FilePath, json);
        }
        catch (Exception e)
        {
            Debug.WriteLine($"[TodoService] Failed to save: {e.Message}");
        }
    }

    #if DEBUG
    private static void OpenDebugFile()
    {
        bool isOpen = Process.GetProcessesByName("notepad").Any(p => p.MainWindowTitle.Contains("data.json"));
        
        if (isOpen) return;
        
        if (File.Exists(FilePath))
            Process.Start("notepad.exe", FilePath);
    }
    #endif
}