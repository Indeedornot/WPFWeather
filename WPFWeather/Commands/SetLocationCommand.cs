using System.Threading.Tasks;

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
        bool validLocation = await _viewModel.ValidateLocation();
        if (!validLocation) {
            _viewModel.IsValidLocation = false;
            return;
        }

        _viewModel.IsValidLocation = true;
        _appStore.SetLocation(_viewModel.Location);
        _weatherHomeNavigationService.Navigate();
    }
}
