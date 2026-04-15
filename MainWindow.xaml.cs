using System.Windows;
using System.Windows.Input;

namespace TodoApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void NewTodoTextBox_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) CreateTodo();
    }

    private void CreateTodo()
    {
        // TODO: Implement todo creation logic, preferable saving the data to a json file
    }
}