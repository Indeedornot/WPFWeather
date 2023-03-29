using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using WPFWeather.Commands;
using WPFWeather.ViewModels;

namespace WPFWeather.Views;

/// <summary>
/// Interaction logic for WeatherHome.xaml
/// </summary>
public partial class WeatherHomeView : UserControl
{
    public WeatherHomeView()
    {
        InitializeComponent();
    }
}