using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TodoApp.Views.Components;

public partial class TodoInput : UserControl
{
    public event EventHandler<string> TodoSubmitted;
    
    public TodoInput()
    {
        InitializeComponent();
    }

    private void Submit_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) SubmitTodo();
    }

    private void Submit(object sender, RoutedEventArgs e) => SubmitTodo();

    private void SubmitTodo()
    {
        string text = Input.Text.Trim();

        if (string.IsNullOrWhiteSpace(text))
            return;
        
        TodoSubmitted?.Invoke(this, text);
        
        Input.Clear();
    }
}