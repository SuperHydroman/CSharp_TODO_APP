using System.Windows.Controls;
using TodoApp.Models;

namespace TodoApp.Views.Pages;

public partial class CompletedList : UserControl
{
    public event EventHandler<string> TodoSubmitted;
    public event EventHandler<Todo> TodoDeleted;
    public event EventHandler<Todo> TodoCompleted; 
    
    public CompletedList()
    {
        InitializeComponent();
    }

    private void OnTodoSubmitted(object sender, string text)
    {
        TodoSubmitted?.Invoke(this, text);
    }

    private void OnTodoDeleted(object sender, Todo todo)
    {
        TodoDeleted?.Invoke(this, todo);
    }

    private void OnTodoCompleted(object sender, Todo todo)
    {
        TodoCompleted?.Invoke(this, todo);
    }
}