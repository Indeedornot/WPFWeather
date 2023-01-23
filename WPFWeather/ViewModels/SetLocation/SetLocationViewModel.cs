using System;

using WPFWeather.Commands;
using WPFWeather.Services.LocationProvider;
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
    private readonly ILocationProvider _locationProvider;
    private readonly NavigationService<WeatherHomeViewModel> _weatherHomeNavigationService;

    public ChooseLocationTypeCommand ChooseLocationTypeCommand { get; }
    public NavigateBackCommand CancelCommand { get; }

    public SetLocationViewModel(ILocationProvider locationProvider, AppStore appStore, NavigationService<WeatherHomeViewModel> weatherHomeNavigationService, NavigationBackService navigationBackService) {
        _weatherHomeNavigationService = weatherHomeNavigationService;
        _locationProvider = locationProvider;
        _appStore = appStore;

        ChooseLocationTypeCommand = new ChooseLocationTypeCommand(this);
        CancelCommand = new NavigateBackCommand(navigationBackService);

        _locationType = "Address";
        _currentViewModel = new SetAddressViewModel(locationProvider, appStore, weatherHomeNavigationService);
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
        return new SetAddressViewModel(_locationProvider, _appStore, _weatherHomeNavigationService);
    }

    private SetZipCodeViewModel createSetZipCodeVM() {
        return new SetZipCodeViewModel(_locationProvider, _appStore, _weatherHomeNavigationService);
    }
}
