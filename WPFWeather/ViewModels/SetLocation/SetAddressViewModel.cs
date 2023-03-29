using System.Threading.Tasks;

using WPFWeather.Commands;
using WPFWeather.Models.LocationInfo;
using WPFWeather.Services.LocationProvider;
using WPFWeather.Services.NavigationService;
using WPFWeather.Services.WeatherProvider;
using WPFWeather.Stores;

namespace WPFWeather.ViewModels.SetLocation;
public class SetAddressViewModel : ViewModelBase, IViewModelSetLocation
{
    private string _cityName = string.Empty;
    public string CityName
    {
        get => _cityName;
        set
        {
            _cityName = value;
            OnPropertyChanged(nameof(CityName));
        }
    }

    private string? _errorMessage = string.Empty;
    public string? ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged(nameof(ErrorMessage));
            OnPropertyChanged(nameof(HasError));
        }
    }

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public SetLocationCommand SumbitCommand { get; }

    private readonly ILocationProvider _locationProvider;
    public SetAddressViewModel(ILocationProvider locationProvider, AppStore appStore, NavigationService<WeatherHomeViewModel> weatherHomeNavigationService)
    {
        _locationProvider = locationProvider;
        SumbitCommand = new SetLocationCommand(this, appStore, weatherHomeNavigationService);
    }

    public async Task<Location?> GetLocation()
    {
        return await _locationProvider.GetLocationByAddressAsync(new Address(CityName));
    }
}