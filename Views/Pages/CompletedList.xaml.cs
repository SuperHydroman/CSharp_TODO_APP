using System.Windows.Controls;
using TodoApp.Models;

namespace TodoApp.Views.Pages;

public partial class CompletedList : UserControl
{
    public event EventHandler<Todo> TodoDeleted;
    public event EventHandler<Todo> TodoRestored; 
    public event EventHandler<string> SearchSubmitted; 
    
    public CompletedList()
    {
        InitializeComponent();
    }

    private void OnTodoDeleted(object sender, Todo todo)
        => TodoDeleted?.Invoke(this, todo);

    private void OnTodoRestored(object sender, Todo todo)
        => TodoRestored?.Invoke(this, todo);
    
    private void OnSearch(object sender, string query)
        => SearchSubmitted?.Invoke(this, query);
}