using System.Threading.Tasks;

using WPFWeather.Stores;

namespace WPFWeather.Commands;
public class LoadWeatherCommand : AsyncCommandBase {
    private readonly AppStore _appStore;

    public LoadWeatherCommand(AppStore appStore) {
        _appStore = appStore;
    }

    public override async Task ExecuteAsync(object parameter) {
        await _appStore.FetchWeather();
    }
}
