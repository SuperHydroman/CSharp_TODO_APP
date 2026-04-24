using System.Windows;
using System.Windows.Controls;

namespace TodoApp.Views.Components;

public partial class UserNav : UserControl
{
    public event EventHandler<string> ViewChanged;
    
    public UserNav()
    {
        InitializeComponent();
    }
    
    private void ChangeView(object sender, RoutedEventArgs e)
    {    
        ViewChanged?.Invoke(this, ((Button)sender).Tag?.ToString()!);
    }
}