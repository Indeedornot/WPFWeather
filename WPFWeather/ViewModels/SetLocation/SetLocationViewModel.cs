using System;

using WPFWeather.Commands;
using WPFWeather.Services.NavigationService;
using WPFWeather.Services.WeatherProvider;
using WPFWeather.Stores;

namespace WPFWeather.ViewModels.SetLocation;
public class SetLocationViewModel : ViewModelBase {
    private string _locationType;
    public string LocationType {
        get => _locationType;
        set {
            _locationType = value;
            ChooseLocationType(_locationType);
            OnPropertyChanged(nameof(LocationType));
        }
    }

    private IViewModelSetLocation _currentViewModel;
    public IViewModelSetLocation CurrentViewModel {
        get => _currentViewModel;
        set {
            _currentViewModel = value;
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }

    private readonly AppStore _appStore;
    private readonly IWeatherProvider _weatherProvider;
    private readonly NavigationService<WeatherHomeViewModel> _weatherHomeNavigationService;

    public ChooseLocationTypeCommand ChooseLocationTypeCommand { get; }
    public NavigateBackCommand CancelCommand { get; }

    public SetLocationViewModel(IWeatherProvider weatherProvider, AppStore appStore, NavigationService<WeatherHomeViewModel> weatherHomeNavigationService, NavigationBackService navigationBackService) {
        _weatherHomeNavigationService = weatherHomeNavigationService;
        _weatherProvider = weatherProvider;
        _appStore = appStore;

        ChooseLocationTypeCommand = new ChooseLocationTypeCommand(this);
        CancelCommand = new NavigateBackCommand(navigationBackService);

        _locationType = "Address";
        _currentViewModel = new SetAddressViewModel(weatherProvider, appStore, weatherHomeNavigationService);
    }

    //For more models I'd go with a DI factory methods
    public void ChooseLocationType(string locationType) {
        CurrentViewModel = locationType switch {
            "Address" => createSetAddressVM(),
            "ZipCode" => createSetZipCodeVM(),
            _ => throw new ArgumentException("Invalid location type"),
        };
    }

    private SetAddressViewModel createSetAddressVM() {
        return new SetAddressViewModel(_weatherProvider, _appStore, _weatherHomeNavigationService);
    }

    private SetZipCodeViewModel createSetZipCodeVM() {
        return new SetZipCodeViewModel(_weatherProvider, _appStore, _weatherHomeNavigationService);
    }
}
