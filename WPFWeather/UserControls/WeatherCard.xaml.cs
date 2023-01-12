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
}