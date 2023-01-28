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

    public ChooseLocationTypeCommand ChooseLocationTypeCommand { get; }
    public NavigateBackCommand CancelCommand { get; }

    private readonly SetAddressViewModel _setAddressViewModel;
    private readonly SetZipCodeViewModel _setZipCodeViewModel;

    public SetLocationViewModel(ILocationProvider locationProvider, AppStore appStore, NavigationService<WeatherHomeViewModel> weatherHomeNavigationService, NavigationBackService navigationBackService) {
        ChooseLocationTypeCommand = new ChooseLocationTypeCommand(this);
        CancelCommand = new NavigateBackCommand(navigationBackService);

        _locationType = "Address";

        _setAddressViewModel = new SetAddressViewModel(locationProvider, appStore, weatherHomeNavigationService);
        _setZipCodeViewModel = new SetZipCodeViewModel(locationProvider, appStore, weatherHomeNavigationService);
        _currentViewModel = _setAddressViewModel;
    }

    //For more models I'd go with a DI factory methods
    public void ChooseLocationType(string locationType) {
        CurrentViewModel = locationType switch {
            "Address" => _setAddressViewModel,
            "ZipCode" => _setZipCodeViewModel,
            _ => throw new ArgumentException("Invalid location type"),
        };
    }
}
