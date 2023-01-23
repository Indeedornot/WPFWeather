using System.Threading.Tasks;

using WPFWeather.Commands;
using WPFWeather.Models.LocationInfo;
using WPFWeather.Services.LocationProvider;
using WPFWeather.Services.NavigationService;
using WPFWeather.Services.WeatherProvider;
using WPFWeather.Stores;

namespace WPFWeather.ViewModels.SetLocation;
public class SetAddressViewModel : ViewModelBase, IViewModelSetLocation {
    private string _cityName = string.Empty;
    public string CityName {
        get => _cityName;
        set {
            _cityName = value;
            OnPropertyChanged(nameof(CityName));
        }
    }

    private bool _isValidLocation;
    public bool IsValidLocation {
        get => _isValidLocation;
        set {
            _isValidLocation = value;
            OnPropertyChanged(nameof(IsValidLocation));
        }
    }

    public SetLocationCommand SumbitCommand { get; }

    private readonly ILocationProvider _locationProvider;
    public SetAddressViewModel(ILocationProvider locationProvider, AppStore appStore, NavigationService<WeatherHomeViewModel> weatherHomeNavigationService) {
        _locationProvider = locationProvider;
        SumbitCommand = new SetLocationCommand(this, appStore, weatherHomeNavigationService);
    }

    public async Task<Location?> GetLocation() {
        return await _locationProvider.GetLocationByAddressAsync(new Address(CityName));
    }
}
