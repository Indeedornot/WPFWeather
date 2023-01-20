using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFWeather.AttachedProp;

//https://stackoverflow.com/a/34284262/11262883
public class ListViewScrollExtensions {
    public static readonly DependencyProperty ScrollChangedCommandProperty =
        DependencyProperty.RegisterAttached(
            "ScrollChangedCommand",
            typeof(ICommand),
            typeof(ListViewScrollExtensions),
            new PropertyMetadata(default(ICommand),
            OnScrollChangedCommandChanged));

    private static void OnScrollChangedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        ListView? listView = d as ListView;
        if (listView == null) return;

        if (e.NewValue != null) {
            listView.Loaded += DataGridOnLoaded;
            return;
        }

        if (e.OldValue != null) {
            listView.Loaded -= DataGridOnLoaded;
            return;
        }
    }

    private static void DataGridOnLoaded(object sender, RoutedEventArgs routedEventArgs) {
        ListView? listView = sender as ListView;
        if (listView == null) return;

        ScrollViewer? scrollViewer = UIHelper.FindChildren<ScrollViewer>(listView).FirstOrDefault();
        if (scrollViewer == null) return;

        scrollViewer.ScrollChanged += ScrollViewerOnScrollChanged;
    }

    private static void ScrollViewerOnScrollChanged(object sender, ScrollChangedEventArgs e) {
        var scrollViewer = sender as ScrollViewer;
        if (scrollViewer == null) return;

        ListView? listView = UIHelper.FindParent<ListView>(scrollViewer);
        if (listView == null) return;

        ICommand command = GetScrollChangedCommand(listView);
        command.Execute(e);
    }

    public static void SetScrollChangedCommand(DependencyObject element, ICommand value) {
        element.SetValue(ScrollChangedCommandProperty, value);
    }

    public static ICommand GetScrollChangedCommand(DependencyObject element) {
        return (ICommand)element.GetValue(ScrollChangedCommandProperty);
    }
}
