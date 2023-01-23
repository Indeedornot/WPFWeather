using System.Threading.Tasks;

using WPFWeather.Commands;
using WPFWeather.Models.LocationInfo;
using WPFWeather.Services.LocationProvider;
using WPFWeather.Services.NavigationService;
using WPFWeather.Services.WeatherProvider;
using WPFWeather.Stores;

namespace WPFWeather.ViewModels.SetLocation;
internal class SetZipCodeViewModel : ViewModelBase, IViewModelSetLocation {
    private string _zipCode = string.Empty;
    public string ZipCode {
        get => _zipCode;
        set {
            _zipCode = value;
            OnPropertyChanged(nameof(ZipCode));
        }
    }

    private string _countryCode = string.Empty;
    public string CountryCode {
        get => _countryCode;
        set {
            _countryCode = value;
            OnPropertyChanged(nameof(CountryCode));
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

    public async Task<Location?> GetLocation() {
        return await _locationProvider.GetLocationByZipCodeAsync(new ZipCode(ZipCode, CountryCode));
    }

    public SetLocationCommand SumbitCommand { get; }

    private readonly ILocationProvider _locationProvider;
    public SetZipCodeViewModel(ILocationProvider locationProvider, AppStore appStore, NavigationService<WeatherHomeViewModel> weatherHomeNavigationService) {
        _locationProvider = locationProvider;
        SumbitCommand = new SetLocationCommand(this, appStore, weatherHomeNavigationService);
    }
}
