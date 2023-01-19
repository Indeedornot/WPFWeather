using System.Windows;
using System.Windows.Controls;

namespace WPFWeather.Views;
/// <summary>
/// Interaction logic for WeatherHome.xaml
/// </summary>
public partial class WeatherHomeView : UserControl {
    public string Message { get; } = "Hi";
    public WeatherHomeView() {
        InitializeComponent();
    }

    public void SayHello(object sender, RoutedEventArgs e) {
        MessageBox.Show(Message);
    }
}
