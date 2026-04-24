using System.Windows;
using System.Windows.Controls;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Views.Components;

public partial class TodoPanel : UserControl
{
    public event EventHandler<Todo> TodoDeleted;  
    
    public TodoPanel()
    {
        InitializeComponent();
    }

    private void Delete(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: Todo todo }) 
            TodoDeleted?.Invoke(this, todo);
    }
}