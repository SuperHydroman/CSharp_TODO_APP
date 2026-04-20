using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
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

    private void AddButton_OnClick(object sender, RoutedEventArgs e) => CreateTodo();

    private void DeleteTodoButton_OnClick(object sender, RoutedEventArgs e)
    {
        Button button = (Button) sender;
        Todo todo = (Todo)button.Tag;
        
        Todos.Remove(todo);
        TodoService.Save(Todos);
    }

    private void CreateTodo()
    {
        string text = NewTodoTextBox.Text;

        if (string.IsNullOrWhiteSpace(text)) return;

        Todo todo = new Todo { Title = text, CreatedAt = DateTime.Now };
        
        Todos.Insert(0, todo);
        TodoService.Save(Todos);
        
        NewTodoTextBox.Clear();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        TodoService.Save(Todos);
        base.OnClosing(e);
    }
}