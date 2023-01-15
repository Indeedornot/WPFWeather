using System.Threading.Tasks;

using WPFWeather.Commands;
using WPFWeather.Models.LocationInfo;
using WPFWeather.Services.NavigationService;
using WPFWeather.Services.WeatherProvider;
using WPFWeather.Stores;

namespace WPFWeather.ViewModels.SetLocation;
public class SetAddressViewModel : ViewModelBase, IViewModelSetLocation {
    public Location Location => new Address(CityName);

    private string _cityName;
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

    private readonly IWeatherProvider _weatherProvider;
    public SetAddressViewModel(IWeatherProvider weatherProvider, AppStore appStore, NavigationService<WeatherHomeViewModel> weatherHomeNavigationService) {
        _weatherProvider = weatherProvider;
        SumbitCommand = new SetLocationCommand(this, appStore, weatherHomeNavigationService);
    }

    public Task<bool> ValidateLocation() {
        return _weatherProvider.ValidateAddressAsync(new Address(CityName));
    }
}
