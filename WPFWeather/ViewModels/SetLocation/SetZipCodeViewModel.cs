using System.Threading.Tasks;

using WPFWeather.Commands;
using WPFWeather.Models.LocationInfo;
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
        return await _weatherProvider.GetLocationByZipCode(new ZipCode(ZipCode, CountryCode));
    }

    public SetLocationCommand SumbitCommand { get; }

    private readonly IWeatherProvider _weatherProvider;
    public SetZipCodeViewModel(IWeatherProvider weatherProvider, AppStore appStore, NavigationService<WeatherHomeViewModel> weatherHomeNavigationService) {
        _weatherProvider = weatherProvider;
        SumbitCommand = new SetLocationCommand(this, appStore, weatherHomeNavigationService);
    }
}
