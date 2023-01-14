using System.Threading.Tasks;

using WPFWeather.Stores;
using WPFWeather.ViewModels;

namespace WPFWeather.Commands;
public class LoadWeatherCommand : AsyncCommandBase {
    private readonly WeatherHomeViewModel _viewModel;
    private readonly AppStore _appStore;

    public LoadWeatherCommand(WeatherHomeViewModel viewModel, AppStore appStore) {
        _viewModel = viewModel;
        _appStore = appStore;
    }

    public override async Task ExecuteAsync(object parameter) {
        await _viewModel.loadData();
    }
}
