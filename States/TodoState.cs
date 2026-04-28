using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.States;

public class TodoState 
{
    public ObservableCollection<Todo>? Todos { get; set; }
    public ObservableCollection<Todo>? CompletedTodos { get; set; }
    public ObservableCollection<Todo>? DeletedTodos { get; set; }

    public void Load()
    {
        List<Todo> all = TodoService.Load();
        
        Todos = new ObservableCollection<Todo>(
            all.Where(t => t is { IsCompleted: false, IsDeleted: false }));
        
        CompletedTodos = new ObservableCollection<Todo>(
            all.Where(t => t is { IsCompleted: true, IsDeleted: false }));
        
        DeletedTodos = new ObservableCollection<Todo>(
            all.Where(t => t.IsDeleted));
    }
    
    public void Add(string text)
    {
        Todos?.Insert(0, new Todo { Title = text, CreatedAt = DateTime.Now });
        Save();
    }

    public void Delete(Todo todo)
    {
        if (todo is { IsDeleted: true, DeletedAt: not null })
        {
            DeletedTodos?.Remove(todo);
            Save();
            return;
        }
        
        todo.IsCompleted = false;
        todo.CompletedAt = null;
        todo.IsDeleted = true;
        todo.DeletedAt = DateTime.Now;

        Todos?.Remove(todo);
        CompletedTodos?.Remove(todo);
        DeletedTodos?.Insert(0, todo);
        Save();
    }

    // TODO: Make sure the Restore puts it back at the right spot based on the datetime, instead of just the top
    public void Restore(Todo todo)
    {
        if (todo.IsCompleted)
        {
            CompletedTodos?.Remove(todo);
        }
        
        if (todo.IsDeleted)
            DeletedTodos?.Remove(todo);
        
        MarkAsActive(todo);
        
        Todos?.Insert(0, todo);
        Save();
    }

    public void Complete(Todo todo)
    {
        todo.IsCompleted = true;
        todo.CompletedAt = DateTime.Now;

        Todos?.Remove(todo);
        CompletedTodos?.Insert(0, todo);
        Save();
    }
    
    public void Save()
    {
        IEnumerable<Todo> fallback = Enumerable.Empty<Todo>();
        
        List<Todo> allTodos = (Todos ?? fallback)
            .Concat(CompletedTodos ?? fallback)
            .Concat(DeletedTodos ?? fallback)
            .ToList();
        
        TodoService.Save(allTodos);
    }
    
    
    public void Search(object? sender, string query, ObservableCollection<Todo> source)
    {
        ICollectionView view = CollectionViewSource.GetDefaultView(source);
        view.Filter = string.IsNullOrWhiteSpace(query)
            ? null
            : obj => obj is Todo t && t.Title.Contains(query, StringComparison.OrdinalIgnoreCase);
    }

    private static void MarkAsActive(Todo todo)
    {
        todo.IsCompleted = false;
        todo.CompletedAt = null;
        todo.IsDeleted = false;
        todo.DeletedAt = null;
    }
}