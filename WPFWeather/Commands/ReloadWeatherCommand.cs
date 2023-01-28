using System.Threading.Tasks;

using WPFWeather.Stores;
using WPFWeather.ViewModels;

namespace WPFWeather.Commands;
public class ReloadWeatherCommand : AsyncCommandBase {
    private readonly AppStore _appStore;
    private WeatherHomeViewModel _viewModel;

    public ReloadWeatherCommand(WeatherHomeViewModel viewModel, AppStore appStore) {
        _appStore = appStore;
        _viewModel = viewModel;
    }

    public override async Task ExecuteAsync(object parameter) {
        _viewModel.TokenSource.Cancel();
        await _appStore.ResetWeather();
        _viewModel.TokenSource.TryReset();
    }
}
