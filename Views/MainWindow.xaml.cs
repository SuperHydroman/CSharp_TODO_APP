using System.Windows;
using TodoApp.Models;
using TodoApp.States;
using TodoApp.Views.Pages;

namespace TodoApp.Views;

public partial class MainWindow : Window
{
    private readonly TodoState _state = new();
    
    // Pages
    private readonly TodoList _todoList = new();
    private readonly CompletedList _completedList = new();
    private readonly DeletedList _deletedList = new();
    
    public MainWindow()
    {
        InitializeComponent();
        
        DataContext = _state;
        _state.Load();
        
        SubscribeEvents();
        MainContent.Content = _todoList;
    }

    private void OnTodoSubmitted(object? sender, string text) 
        => _state.Add(text);

    private void OnTodoCompleted(object? sender, Todo todo)
        => _state.Complete(todo);
    
    private void OnTodoDeleted(object? sender, Todo todo)
        => _state.Delete(todo);
    
    private void OnTodoRestored(object? sender, Todo todo)
        => _state.Restore(todo);
        
    private void OnCompletedSearch(object? sender, string query) 
        => _state.Search(sender, query, _state.CompletedTodos!);
        
    private void OnDeletedSearch(object? sender, string query)
        => _state.Search(sender, query, _state.DeletedTodos!);

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
        _state.Save();
        base.OnClosed(e);
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
    
}