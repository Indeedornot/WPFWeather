using System;
using System.Windows;
using System.Windows.Controls;

using WPFWeather.Models;

namespace WPFWeather.UserControls;

/// <summary>
/// Interaction logic for WeatherCard.xaml
/// </summary>
public partial class WeatherCard : UserControl {
    public WeatherCard() {
        InitializeComponent();
    }

    public string TimeString => Time.ToString("HH:mm");
    public string DateString => Time.ToString("dd-MM");

    public string Temperature {
        get => (string)GetValue(TemperatureProperty);
        set => SetValue(TemperatureProperty, value);
    }

    public DateTime Time {
        get => (DateTime)GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }

    public bool Loading {
        get => (bool)GetValue(LoadingProperty);
        set => SetValue(LoadingProperty, value);
    }

    public WeatherType Description {
        get => (WeatherType)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }


    public static readonly DependencyProperty TimeProperty =
        DependencyProperty.Register(nameof(Time), typeof(DateTime), typeof(WeatherCard), new PropertyMetadata(default(DateTime)));

    public static readonly DependencyProperty TemperatureProperty =
        DependencyProperty.Register(nameof(Temperature), typeof(string), typeof(WeatherCard), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty LoadingProperty =
        DependencyProperty.Register(nameof(Loading), typeof(bool), typeof(WeatherCard), new PropertyMetadata(default(bool)));

    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register(nameof(Description), typeof(WeatherType), typeof(WeatherCard), new PropertyMetadata(WeatherType.Clear));


}