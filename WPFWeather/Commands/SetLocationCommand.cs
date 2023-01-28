using System.Threading.Tasks;

using WPFWeather.Models.LocationInfo;
using WPFWeather.Services.NavigationService;
using WPFWeather.Stores;
using WPFWeather.ViewModels;
using WPFWeather.ViewModels.SetLocation;

namespace WPFWeather.Commands;
public class SetLocationCommand : AsyncCommandBase {
    private readonly IViewModelSetLocation _viewModel;
    private readonly NavigationService<WeatherHomeViewModel> _weatherHomeNavigationService;
    private readonly AppStore _appStore;

    public SetLocationCommand(IViewModelSetLocation viewModel, AppStore appStore, NavigationService<WeatherHomeViewModel> weatherHomeNavigationService) {
        _viewModel = viewModel;
        _weatherHomeNavigationService = weatherHomeNavigationService;
        _appStore = appStore;
    }

    public override async Task ExecuteAsync(object parameter) {
        Location? location = await _viewModel.GetLocation();
        if (location == null) {
            _viewModel.ErrorMessage = "Location not found";
            return;
        }

        _appStore.SetLocation(location);
        _weatherHomeNavigationService.Navigate();
    }
}
