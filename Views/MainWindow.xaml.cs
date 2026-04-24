using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using TodoApp.Models;
using TodoApp.Services;
using TodoApp.Views.Pages;

namespace TodoApp.Views;

public partial class MainWindow : Window
{
    public ObservableCollection<Todo> Todos { get; set; }
    public ObservableCollection<Todo> CompletedTodos { get; set; }
    public ObservableCollection<Todo> DeletedTodos { get; set; }
    
    // Pages
    private readonly TodoList _todoList = new();
    private readonly CompletedList _completedList = new();
    private readonly DeletedList _deletedList = new();
    
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        LoadTodoData();
        
        // Wire up the events
        _todoList.TodoSubmitted += OnTodoSubmitted;
        _todoList.TodoDeleted += OnTodoDeleted;
        _todoList.TodoCompleted += OnTodoCompleted;
        
        // Set default page on start
        MainContent.Content = _todoList;
    }

    private void OnTodoSubmitted(object? sender, string text)
    {
        Todos.Insert(0, new Todo { Title = text, CreatedAt = DateTime.Now });
        SaveAll();
    }

    private void OnTodoCompleted(object? sender, Todo todo)
    {
        todo.IsCompleted = true;
        todo.CompletedAt = DateTime.Now;
        SaveAll();
    }
    
    private void OnTodoDeleted(object? sender, Todo todo)
    {
        todo.IsCompleted = false;
        todo.CompletedAt = null;
        todo.IsDeleted = true;
        todo.DeletedAt = DateTime.Now;
        SaveAll();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        SaveAll();
        base.OnClosing(e);
    }

    private void OnViewChanged(object sender, string view)
    {
        MainContent.Content = view switch
        {
            "todos" => _todoList,
            "completed" => _completedList,
            "deleted" => _deletedList,
            _ => _todoList
        };
    }

    private void LoadTodoData()
    {
        List<Todo> allTodos = TodoService.Load();
        
        Todos = new ObservableCollection<Todo>(
            allTodos.Where(t => !t.IsCompleted && !t.IsDeleted));
        
        CompletedTodos = new ObservableCollection<Todo>(
            allTodos.Where(t => t.IsCompleted && !t.IsDeleted));
        
        DeletedTodos = new ObservableCollection<Todo>(
            allTodos.Where(t => t.IsDeleted));
    }
    
    private void SaveAll()
    {
        List<Todo> allTodos = Todos
            .Concat(CompletedTodos)
            .Concat(DeletedTodos)
            .ToList();
        
        TodoService.Save(allTodos);
    }
}