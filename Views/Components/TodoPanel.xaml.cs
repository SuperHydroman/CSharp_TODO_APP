using System.Windows;
using System.Windows.Controls;
using TodoApp.Models;

namespace TodoApp.Views.Components;

public partial class TodoPanel : UserControl
{
    public event EventHandler<Todo> TodoDeleted;  
    public event EventHandler<Todo> TodoCompleted;  
    
    public TodoPanel()
    {
        InitializeComponent();
    }

    private void Delete(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: Todo todo }) 
            TodoDeleted?.Invoke(this, todo);
    }
    private void Complete(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: Todo todo }) 
            TodoCompleted?.Invoke(this, todo);
    }
}