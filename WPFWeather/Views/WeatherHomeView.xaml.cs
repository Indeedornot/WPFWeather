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
public partial class WeatherHomeView : UserControl {
    public RelayCommand HitMaxCommand { get; }
    public RelayCommand HitMinCommand { get; }

    public WeatherHomeView() {
        InitializeComponent();

        HitMaxCommand = new(HitMax);
        HitMinCommand = new(HitMin);
    }

    private void HitMax(object parameter) {
        Debug.WriteLine("HitMax");
    }

    private void HitMin(object parameter) {
        Debug.WriteLine("HitMin");
    }
}
