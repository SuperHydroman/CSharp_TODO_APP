using System.Collections;
using System.Windows;

namespace TodoApp.Views.Components;

public partial class TodoPanel
{
    // Configurable item source
    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable),
            typeof(TodoPanel), new PropertyMetadata(null));

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    
    public static readonly DependencyProperty ShowRestoreButtonProperty = DependencyProperty.Register(
        nameof(ShowRestoreButton),
        typeof(bool),
        typeof(TodoPanel),
        new PropertyMetadata(false)
    );
    
    public bool ShowRestoreButton
    {
        get => (bool)GetValue(ShowRestoreButtonProperty);
        set => SetValue(ShowRestoreButtonProperty, value);
    }
    
    public static readonly DependencyProperty ShowCompleteButtonProperty = DependencyProperty.Register(
        nameof(ShowCompleteButton),
        typeof(bool),
        typeof(TodoPanel),
        new PropertyMetadata(false)
    );
    
    public bool ShowCompleteButton
    {
        get => (bool)GetValue(ShowCompleteButtonProperty);
        set => SetValue(ShowCompleteButtonProperty, value);
    }
    
    public static readonly DependencyProperty ShowDeleteButtonProperty = DependencyProperty.Register(
        nameof(ShowDeleteButton),
        typeof(bool),
        typeof(TodoPanel),
        new PropertyMetadata(false)
    );
    
    public bool ShowDeleteButton
    {
        get => (bool)GetValue(ShowDeleteButtonProperty);
        set => SetValue(ShowDeleteButtonProperty, value);
    }

    public static readonly DependencyProperty IsStrikethroughProperty = DependencyProperty.Register(
        nameof(IsStrikethrough),
        typeof(bool),
        typeof(TodoPanel),
        new PropertyMetadata(false)
    );

    public bool IsStrikethrough
    {
        get => (bool)GetValue(IsStrikethroughProperty);
        set => SetValue(IsStrikethroughProperty, value);
    }
}