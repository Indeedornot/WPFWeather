using System.Threading.Tasks;

using WPFWeather.Stores;

namespace WPFWeather.Commands;
public class ReloadWeatherCommand : AsyncCommandBase {
    private readonly AppStore _appStore;

    public ReloadWeatherCommand(AppStore appStore) {
        _appStore = appStore;
    }

    public override async Task ExecuteAsync(object parameter) {
        await _appStore.ResetWeather();
    }
}
