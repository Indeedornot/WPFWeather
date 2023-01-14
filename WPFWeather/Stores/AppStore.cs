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
    public bool IsLoaded { get; internal set; }

    public event Action<bool> LoadingChanged;
    public event Action<bool> LoadedChanged;
    public event Action<IEnumerable<WeatherData>> WeatherForecastsChanged;
    public event Action<Location?> LocationChanged;

    private readonly IWeatherProvider _weatherProvider;
    public AppStore(IWeatherProvider weatherProvider, PersistentData? persistentData) {
        _weatherProvider = weatherProvider;

        _location = persistentData?.Location;

        _weatherForecasts = new();
        _initializeLazy = new Lazy<Task>(FetchWeather(_location));
    }

    public void SetLocation(Location? location) {
        _location = location;
        LocationChanged?.Invoke(location);

        //TODO: ADD CANCELING
        Load();
    }

    public void SetWeather(IEnumerable<WeatherData> weather) {
        _weatherForecasts = weather.ToList();
        WeatherForecastsChanged?.Invoke(weather);
    }

    private async Task FetchWeather(Location? location) {
        if (location is not (ZipCode or Address)) {
            _weatherForecasts.Clear();
            WeatherForecastsChanged?.Invoke(_weatherForecasts);
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
    }

    public async Task Load() {
        try {
            setLoading(true);
            setLoaded(false);
            await _initializeLazy.Value;
            setLoading(false);
            setLoaded(true);

            //we restart the process every load
            _initializeLazy = new Lazy<Task>(FetchWeather(_location));
        }
        catch (Exception) {
            setLoading(false);
            setLoaded(false);
            _initializeLazy = new Lazy<Task>(FetchWeather(_location));
            throw;
        }
    }

    private void setLoading(bool value) {
        if (IsLoading == value) return;

        IsLoading = value;
        LoadingChanged?.Invoke(value);
    }

    private void setLoaded(bool value) {
        if (IsLoaded == value) return;

        IsLoaded = value;
        LoadedChanged?.Invoke(value);
    }
}
