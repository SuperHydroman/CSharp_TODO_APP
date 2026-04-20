using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
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

    private void NewTodoTextBox_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) CreateTodo();
    }

    private void AddButton_OnClick(object sender, RoutedEventArgs e)
    {
        CreateTodo();
    }

    private void CreateTodo()
    {
        string text = NewTodoTextBox.Text;

        if (string.IsNullOrWhiteSpace(text)) return;
        
        // Todos.Add(new Todo { Title = text, Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit." });
        Todos.Add(new Todo { Title = text });
        TodoService.Save(Todos);
        
        NewTodoTextBox.Clear();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        TodoService.Save(Todos);
        base.OnClosing(e);
    }
}