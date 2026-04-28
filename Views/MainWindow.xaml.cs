using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using TodoApp.Models;
using TodoApp.Services;
using TodoApp.Views.Pages;

namespace TodoApp.Views;

public partial class MainWindow : Window
{
    public ObservableCollection<Todo>? Todos { get; set; }
    public ObservableCollection<Todo>? CompletedTodos { get; set; }
    public ObservableCollection<Todo>? DeletedTodos { get; set; }
    
    // Pages
    private readonly TodoList _todoList = new();
    private readonly CompletedList _completedList = new();
    private readonly DeletedList _deletedList = new();
    
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        LoadTodoData();
        
        SubscribeEvents();
        
        // Set the default page
        MainContent.Content = _todoList;
    }

    private void SubscribeEvents()
    {
        _todoList.TodoSubmitted += OnTodoSubmitted;
        _todoList.TodoDeleted += OnTodoDeleted;
        _todoList.TodoCompleted += OnTodoCompleted;
        
        _completedList.TodoRestored += OnTodoRestored;
        _completedList.TodoDeleted += OnTodoDeleted;
        _completedList.SearchSubmitted += OnCompletedSearch;
        
        _deletedList.TodoRestored += OnTodoRestored;
        _deletedList.TodoDeleted += OnTodoDeleted;
        _deletedList.SearchSubmitted += OnDeletedSearch;
    }
    
    private void UnSubscribeEvents()
    {
        _todoList.TodoSubmitted -= OnTodoSubmitted;
        _todoList.TodoDeleted -= OnTodoDeleted;
        _todoList.TodoCompleted -= OnTodoCompleted;
        
        _completedList.TodoRestored -= OnTodoRestored;
        _completedList.TodoDeleted -= OnTodoDeleted;
        _completedList.SearchSubmitted -= OnCompletedSearch;
        
        _deletedList.TodoRestored -= OnTodoRestored;
        _deletedList.TodoDeleted -= OnTodoDeleted;
        _deletedList.SearchSubmitted -= OnDeletedSearch;
    }

    private void OnTodoSubmitted(object? sender, string text)
    {
        Todos?.Insert(0, new Todo { Title = text, CreatedAt = DateTime.Now });
        SaveAll();
    }

    private void OnTodoCompleted(object? sender, Todo todo)
    {
        todo.IsCompleted = true;
        todo.CompletedAt = DateTime.Now;

        Todos?.Remove(todo);
        CompletedTodos?.Insert(0, todo);
        
        SaveAll();
    }
    
    private void OnTodoDeleted(object? sender, Todo todo)
    {
        if (todo is { IsDeleted: true, DeletedAt: not null })
        {
            RemoveTodo(todo);
            return;
        }
        
        todo.IsCompleted = false;
        todo.CompletedAt = null;
        todo.IsDeleted = true;
        todo.DeletedAt = DateTime.Now;

        Todos?.Remove(todo);
        CompletedTodos?.Remove(todo);
        DeletedTodos?.Insert(0, todo);
        
        SaveAll();
    }

    private void OnTodoRestored(object? sender, Todo todo)
    {
        if (todo is { IsCompleted: true, CompletedAt: not null })
        {
            todo.IsCompleted = false;
            todo.CompletedAt = null;
            CompletedTodos?.Remove(todo);
        } else if (todo is { IsDeleted: true, DeletedAt: not null })
        {
            todo.IsDeleted = false;
            todo.DeletedAt = null;
            DeletedTodos?.Remove(todo);
        }
        
        Todos?.Insert(0, todo);
        SaveAll();
    }

    private static void OnSearch(object? sender, string query, ObservableCollection<Todo> source)
    {
        ICollectionView view = CollectionViewSource.GetDefaultView(source);
        view.Filter = string.IsNullOrWhiteSpace(query)
            ? null
            : obj => obj is Todo t && t.Title.Contains(query, StringComparison.OrdinalIgnoreCase);
    }

    private void OnViewChanged(object? sender, string view)
    {
        MainContent.Content = view switch
        {
            "todos" => _todoList,
            "completed" => _completedList,
            "deleted" => _deletedList,
            _ => _todoList
        };
    }
    
    protected override void OnClosed(EventArgs e)
    {
        UnSubscribeEvents();
        SaveAll();
        base.OnClosed(e);
    }

    private void LoadTodoData()
    {
        List<Todo> allTodos = TodoService.Load();
        
        Todos = new ObservableCollection<Todo>(
            allTodos.Where(t => t is { IsCompleted: false, IsDeleted: false }));
        
        CompletedTodos = new ObservableCollection<Todo>(
            allTodos.Where(t => t is { IsCompleted: true, IsDeleted: false }));
        
        DeletedTodos = new ObservableCollection<Todo>(
            allTodos.Where(t => t.IsDeleted));
    }
    
    private void SaveAll()
    {
        IEnumerable<Todo> fallback = Enumerable.Empty<Todo>();
        
        List<Todo> allTodos = (Todos ?? fallback)
            .Concat(CompletedTodos ?? fallback)
            .Concat(DeletedTodos ?? fallback)
            .ToList();
        
        TodoService.Save(allTodos);
    }
    
    private void RemoveTodo(Todo todo)
    {
        DeletedTodos?.Remove(todo);
        SaveAll();
    }
        
    private void OnCompletedSearch(object? sender, string query) 
        => OnSearch(sender, query, CompletedTodos!);
        
    private void OnDeletedSearch(object? sender, string query)
        => OnSearch(sender, query, DeletedTodos!);
    
}