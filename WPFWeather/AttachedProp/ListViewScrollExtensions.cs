using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFWeather.AttachedProp;

//https://stackoverflow.com/a/34284262/11262883
//Bug: Double registers events
public class ListViewScrollExtensions {
    #region ScrollHitMaxCommand
    public static readonly DependencyProperty ScrollHitMaxCommandProperty =
        DependencyProperty.RegisterAttached(
            "ScrollHitMaxCommand",
            typeof(ICommand),
            typeof(ListViewScrollExtensions),
            new PropertyMetadata(default(ICommand),
            OnScrollChangedCommandChanged));

    public static void SetScrollHitMaxCommand(DependencyObject element, ICommand value) {
        element.SetValue(ScrollHitMaxCommandProperty, value);
    }
    public static ICommand GetScrollHitMaxCommand(DependencyObject element) {
        return (ICommand)element.GetValue(ScrollHitMaxCommandProperty);
    }
    #endregion
    #region ScrollHitMinCommand
    public static readonly DependencyProperty ScrollHitMinCommandProperty =
        DependencyProperty.RegisterAttached(
            "ScrollHitMinCommand",
            typeof(ICommand),
            typeof(ListViewScrollExtensions),
            new PropertyMetadata(default(ICommand),
            OnScrollChangedCommandChanged));

    public static void SetScrollHitMinCommand(DependencyObject element, ICommand value) {
        element.SetValue(ScrollHitMinCommandProperty, value);
    }
    public static ICommand GetScrollHitMinCommand(DependencyObject element) {
        return (ICommand)element.GetValue(ScrollHitMinCommandProperty);
    }
    #endregion


    private static void OnScrollChangedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        ListView? listView = d as ListView;
        if (listView == null) return;

        if (e.NewValue != null) {
            listView.Loaded += ListViewOnLoaded;
            return;
        }

        if (e.OldValue != null) {
            listView.Loaded -= ListViewOnLoaded;
            return;
        }
    }

    private static void ListViewOnLoaded(object sender, RoutedEventArgs routedEventArgs) {
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

        HandleScrollChanged(listView, e);
        //command.Execute(e);
    }

    private static void HandleScrollChanged(ListView listView, ScrollChangedEventArgs e) {
        ICommand MaxHitCommand = GetScrollHitMaxCommand(listView);
        ICommand MinHitCommand = GetScrollHitMinCommand(listView);

        // && e.VerticalChange == 0
        if (e.HorizontalChange == 0) return;

        if (e.HorizontalOffset == 0) {
            MinHitCommand.Execute(e);
            return;
        }

        if (e.HorizontalOffset == e.ExtentWidth - e.ViewportWidth) {
            MaxHitCommand.Execute(e);
            return;
        }


        //if (e.VerticalChange != 0) {
        //    if (e.VerticalOffset == 0) {
        //        MinHitCommand.Execute(e);
        //    }
        //    else if (e.VerticalOffset == e.ViewportHeight - e.ExtentHeight) {
        //        MaxHitCommand.Execute(e);
        //    }
        //}
    }
}
