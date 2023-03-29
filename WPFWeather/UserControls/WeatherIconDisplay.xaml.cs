using System.Windows;
using System.Windows.Controls;

using WPFWeather.Models;

namespace WPFWeather.UserControls;
/// <summary>
/// Interaction logic for WeatherIcon.xaml
/// </summary>
public partial class WeatherIconDisplay : UserControl
{
    public WeatherIconDisplay()
    {
        InitializeComponent();
    }

    public WeatherType Description
    {
        get => (WeatherType)GetValue(WeatherTypeProperty);
        set => SetValue(WeatherTypeProperty, value);
    }

    public static readonly DependencyProperty WeatherTypeProperty =
        DependencyProperty.Register(nameof(Description), typeof(WeatherType), typeof(WeatherIconDisplay), new PropertyMetadata(WeatherType.Clear));
}