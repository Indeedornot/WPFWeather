using System.Windows;
using System.Windows.Controls;

namespace WPFWeather.UserControls;

/// <summary>
/// Interaction logic for InfoCard.xaml
/// </summary>
public partial class InfoCard : UserControl {
    public InfoCard() {
        InitializeComponent();
    }

    public string Info {
        get => (string)GetValue(InfoProperty);
        set => SetValue(InfoProperty, value);
    }

    public string Text {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public Control Icon {
        get => (Control)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    // Using a DependencyProperty as the backing store for Info.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty InfoProperty =
        DependencyProperty.Register(nameof(Info), typeof(string), typeof(InfoCard), new PropertyMetadata(default(string)));


    // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(InfoCard), new PropertyMetadata(default(string)));

    // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(Control), typeof(InfoCard), new PropertyMetadata(default(Control)));


}
