using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WPFWeather.Models;
using WPFWeather.Models.LocationInfo;
using WPFWeather.Services.WeatherProvider;

namespace WPFWeather.Stores;

public class AppStore {
    private List<WeatherData> _weatherForecasts;
    public IEnumerable<WeatherData> WeatherForecasts => _weatherForecasts;

    private Location? _location;
    public Location? Location => _location;

    private Lazy<Task> _initializeLazy;
    public bool IsLoading { get; internal set; }

    public event Action<bool> LoadingChanged;
    public event Action<IEnumerable<WeatherData>> WeatherForecastsChanged;
    public event Action<Location?> LocationChanged;

    private readonly IWeatherProvider _weatherProvider;
    public AppStore(IWeatherProvider weatherProvider, PersistentData? persistentData) {
        _weatherProvider = weatherProvider;

        _location = persistentData?.Location;

        _weatherForecasts = new();
        _initializeLazy = new Lazy<Task>(FetchWeather(_location));
    }

    private async Task FetchWeather(Location? location) {
        setLoading(true);

        if (location is not (ZipCode or Address)) {
            _weatherForecasts.Clear();
            WeatherForecastsChanged?.Invoke(_weatherForecasts);
            setLoading(false);
            return;
        }

        IEnumerable<WeatherData> weather;
        if (location is ZipCode zipCode) {
            weather = await _weatherProvider.GetWeatherAsync(zipCode);
            _weatherForecasts = weather.ToList();
        }
        else if (location is Address address) {
            weather = await _weatherProvider.GetWeatherAsync(address);
            _weatherForecasts = weather.ToList();
        }

        WeatherForecastsChanged?.Invoke(_weatherForecasts);
        setLoading(false);
    }

    private void setLoading(bool value) {
        IsLoading = value;
        LoadingChanged?.Invoke(value);
    }

    public async Task Load() {
        try {
            await _initializeLazy.Value;
        }
        catch (Exception) {
            _initializeLazy = new Lazy<Task>(FetchWeather(_location));
            throw;
        }
    }

    public void SetLocation(Location? location) {
        _location = location;
        LocationChanged?.Invoke(location);

        //TODO: ADD CANCELING
        FetchWeather(location);
    }

    public void SetWeather(IEnumerable<WeatherData> weather) {
        _weatherForecasts = weather.ToList();
        WeatherForecastsChanged?.Invoke(weather);
    }
}
