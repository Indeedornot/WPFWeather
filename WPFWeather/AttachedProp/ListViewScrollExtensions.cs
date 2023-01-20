using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFWeather.AttachedProp;

//https://stackoverflow.com/a/34284262/11262883
//Bug: Double registers events
public class ListViewScrollExtensions {
    #region ScrollHitXMaxCommand
    public static readonly DependencyProperty ScrollHitXMaxCommandProperty =
        DependencyProperty.RegisterAttached(
            "ScrollHitXMaxCommand",
            typeof(ICommand),
            typeof(ListViewScrollExtensions),
            new PropertyMetadata(default(ICommand),
            OnScrollChangedCommandChanged));

    public static void SetScrollHitXMaxCommand(DependencyObject element, ICommand value) {
        element.SetValue(ScrollHitXMaxCommandProperty, value);
    }
    public static ICommand GetScrollHitXMaxCommand(DependencyObject element) {
        return (ICommand)element.GetValue(ScrollHitXMaxCommandProperty);
    }
    #endregion
    #region ScrollHitXMinCommand
    public static readonly DependencyProperty ScrollHitXMinCommandProperty =
        DependencyProperty.RegisterAttached(
            "ScrollHitXMinCommand",
            typeof(ICommand),
            typeof(ListViewScrollExtensions),
            new PropertyMetadata(default(ICommand),
            OnScrollChangedCommandChanged));

    public static void SetScrollHitXMinCommand(DependencyObject element, ICommand value) {
        element.SetValue(ScrollHitXMinCommandProperty, value);
    }
    public static ICommand GetScrollHitXMinCommand(DependencyObject element) {
        return (ICommand)element.GetValue(ScrollHitXMinCommandProperty);
    }
    #endregion
    #region ScrollHitYMaxCommand
    public static readonly DependencyProperty ScrollHitYMaxCommandProperty =
        DependencyProperty.RegisterAttached(
            "ScrollHitYMaxCommand",
            typeof(ICommand),
            typeof(ListViewScrollExtensions),
            new PropertyMetadata(default(ICommand),
            OnScrollChangedCommandChanged));

    public static void SetScrollHitYMaxCommand(DependencyObject element, ICommand value) {
        element.SetValue(ScrollHitYMaxCommandProperty, value);
    }
    public static ICommand GetScrollHitYMaxCommand(DependencyObject element) {
        return (ICommand)element.GetValue(ScrollHitYMaxCommandProperty);
    }
    #endregion
    #region ScrollHitYMinCommand
    public static readonly DependencyProperty ScrollHitYMinCommandProperty =
        DependencyProperty.RegisterAttached(
            "ScrollHitYMinCommand",
            typeof(ICommand),
            typeof(ListViewScrollExtensions),
            new PropertyMetadata(default(ICommand),
            OnScrollChangedCommandChanged));

    public static void SetScrollHitYMinCommand(DependencyObject element, ICommand value) {
        element.SetValue(ScrollHitYMinCommandProperty, value);
    }
    public static ICommand GetScrollHitYMinCommand(DependencyObject element) {
        return (ICommand)element.GetValue(ScrollHitYMinCommandProperty);
    }
    #endregion

    //Commands changed (and whether we need to listen for reloads)
    private static int _commandsRegistered = 0;
    private static void OnScrollChangedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        ListView? listView = d as ListView;
        if (listView == null) return;

        int oldCommandCount = _commandsRegistered;

        //command added
        if (e.NewValue != null && e.OldValue == null) {
            _commandsRegistered += 1;
        }
        //command removed
        else if (e.NewValue == null && e.OldValue != null) {
            _commandsRegistered -= 1;
        }

        //first new command added
        if (_commandsRegistered > 0 && oldCommandCount == 0) {
            listView.Loaded += ListViewOnLoaded;
            return;
        }
        //all commands removed
        else if (_commandsRegistered == 0 && oldCommandCount > 0) {
            listView.Loaded -= ListViewOnLoaded;
            return;
        }
    }

    //ListView (re)loaded
    private static void ListViewOnLoaded(object sender, RoutedEventArgs routedEventArgs) {
        ListView? listView = sender as ListView;
        if (listView == null) return;

        ScrollViewer? scrollViewer = UIHelper.FindChildren<ScrollViewer>(listView).FirstOrDefault();
        if (scrollViewer == null) return;

        scrollViewer.ScrollChanged += ScrollViewerOnScrollChanged;
    }

    //Handle scroll changed
    private static void ScrollViewerOnScrollChanged(object sender, ScrollChangedEventArgs e) {
        var scrollViewer = sender as ScrollViewer;
        if (scrollViewer == null) return;

        ListView? listView = UIHelper.FindParent<ListView>(scrollViewer);
        if (listView == null) return;

        HandleScrollChanged(listView, e);
        //command.Execute(e);
    }

    private static void HandleScrollChanged(ListView listView, ScrollChangedEventArgs e) {
        ICommand MaxXHitCommand = GetScrollHitXMaxCommand(listView);
        ICommand MinXHitCommand = GetScrollHitXMinCommand(listView);

        ICommand MaxYHitCommand = GetScrollHitYMaxCommand(listView);
        ICommand MinYHitCommand = GetScrollHitYMinCommand(listView);

        // && e.VerticalChange == 0
        if (e.HorizontalChange == 0 && e.VerticalChange == 0) return;

        if (e.HorizontalChange != 0) {
            if (e.HorizontalOffset == 0) {
                MinXHitCommand.Execute(e);
            }

            if (e.HorizontalOffset == e.ExtentWidth - e.ViewportWidth) {
                MaxXHitCommand.Execute(e);
            }
        }

        if (e.VerticalChange != 0) {
            if (e.VerticalOffset == 0) {
                MinYHitCommand.Execute(e);
            }

            if (e.VerticalOffset == e.ExtentHeight - e.ViewportHeight) {
                MaxYHitCommand.Execute(e);
            }
        }
    }
}
