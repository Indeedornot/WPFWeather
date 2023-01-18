using System.Windows;
using System.Windows.Controls;

namespace WPFWeather.UserControls;

/// <summary>
/// Interaction logic for WeatherCard.xaml
/// </summary>
public partial class WeatherCard : UserControl {
    public WeatherCard() {
        InitializeComponent();
    }

    public string Time {
        get => (string)GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }

    public string Temperature {
        get => (string)GetValue(TemperatureProperty);
        set => SetValue(TemperatureProperty, value);
    }

    public static readonly DependencyProperty TimeProperty =
        DependencyProperty.Register(nameof(Time), typeof(string), typeof(WeatherCard), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty TemperatureProperty =
        DependencyProperty.Register(nameof(Temperature), typeof(string), typeof(WeatherCard), new PropertyMetadata(default(string)));



    public bool Loading {
        get => (bool)GetValue(LoadingProperty);
        set => SetValue(LoadingProperty, value);
    }

    // Using a DependencyProperty as the backing store for Loading.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty LoadingProperty =
        DependencyProperty.Register(nameof(Loading), typeof(bool), typeof(WeatherCard), new PropertyMetadata(default(bool)));


}