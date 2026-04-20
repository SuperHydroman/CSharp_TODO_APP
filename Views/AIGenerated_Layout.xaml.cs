using System.Windows;
using System.Windows.Input;

namespace TodoApp.Views
{
    public partial class AIGenerated_Layout : Window
    {
        public AIGenerated_Layout()
        {
            InitializeComponent();
        }

        private void AIGenerated_Layout_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void MinimizeButton_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Placeholder handlers — wire up your logic later
        private void NewTodoTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
        }
    }
}