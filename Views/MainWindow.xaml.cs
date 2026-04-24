using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Views;

public partial class MainWindow : Window
{
    public ObservableCollection<Todo> Todos { get; set; }
    
    public MainWindow()
    {
        InitializeComponent();
        
        Todos = new ObservableCollection<Todo>(TodoService.Load());
        
        DataContext = this;
    }

    private void OnTodoSubmitted(object sender, string text)
    {
        Todos.Insert(0, new Todo { Title = text, CreatedAt = DateTime.Now });
        TodoService.Save(Todos);
    }

    private void OnTodoDeleted(object sender, Todo todo)
    {
        Todos.Remove(todo);
        TodoService.Save(Todos);
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        TodoService.Save(Todos);
        base.OnClosing(e);
    }
}