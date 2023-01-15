using System.Threading.Tasks;

using WPFWeather.Commands;
using WPFWeather.Models.LocationInfo;
using WPFWeather.Services.NavigationService;
using WPFWeather.Services.WeatherProvider;
using WPFWeather.Stores;

namespace WPFWeather.ViewModels.SetLocation;
internal class SetZipCodeViewModel : ViewModelBase, IViewModelSetLocation {
    private string _zipCode;
    public string ZipCode {
        get => _zipCode;
        set {
            _zipCode = value;
            OnPropertyChanged(nameof(ZipCode));
        }
    }

    private string _countryCode;
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

    public Location Location => new ZipCode(ZipCode, CountryCode);

    public SetLocationCommand SumbitCommand { get; }

    private IWeatherProvider _weatherProvider;
    public SetZipCodeViewModel(IWeatherProvider weatherProvider, AppStore appStore, NavigationService<WeatherHomeViewModel> weatherHomeNavigationService) {
        _weatherProvider = weatherProvider;
        SumbitCommand = new SetLocationCommand(this, appStore, weatherHomeNavigationService);
    }

    public Task<bool> ValidateLocation() {
        return _weatherProvider.ValidateZipCodeAsync(new ZipCode(ZipCode, CountryCode));
    }
}
