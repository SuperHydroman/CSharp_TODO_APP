using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TodoApp.Views.Components;

public partial class TodoSearch : UserControl
{
    public event EventHandler<string> SearchSubmitted;
    
    public TodoSearch()
    {
        InitializeComponent();
    }

    private void Search_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) Search(sender, e);
    }

    private void Search(object sender, RoutedEventArgs e)
    {
        string query = Input.Text.Trim();
        SearchSubmitted?.Invoke(this, query);
    }
}