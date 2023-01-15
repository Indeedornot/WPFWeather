using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WPFWeather.Models;
using WPFWeather.Models.LocationInfo;
using WPFWeather.Services.Provider;
using WPFWeather.Services.WeatherProvider;

namespace WPFWeather.Stores;

public class AppStore {
    private List<WeatherData> _weatherForecasts;
    public IEnumerable<WeatherData> WeatherForecasts => _weatherForecasts;

    private Location? _location;
    public Location? Location => _location;

    public bool IsLoading { get; internal set; }

    private Lazy<Task> _initializeTask;
    public bool IsInitialized => _initializeTask.IsValueCreated;

    public event Action<bool> LoadingChanged;
    public event Action<IEnumerable<WeatherData>> WeatherForecastsChanged;
    public event Action<Location?> LocationChanged;

    private readonly IWeatherProvider _weatherProvider;
    private readonly IPersistentDataManager _dataManager;
    public AppStore(IWeatherProvider weatherProvider, IPersistentDataManager dataManager) {
        _weatherProvider = weatherProvider;
        _dataManager = dataManager;

        _weatherForecasts = new();
        _initializeTask = new(initialize);
    }

    public void SetLocation(Location? location) {
        _location = location;
        LocationChanged?.Invoke(location);

        //TODO: ADD CANCELING
        FetchWeather();
    }

    public void SetWeather(IEnumerable<WeatherData> weather) {
        _weatherForecasts = weather.ToList();
        WeatherForecastsChanged?.Invoke(weather);
    }

    public async Task FetchWeather() {
        if (Location == null) return;

        setLoading(true);
        if (_location is not (ZipCode or Address)) {
            _weatherForecasts.Clear();
            WeatherForecastsChanged?.Invoke(_weatherForecasts);
            setLoading(false);
            return;
        }

        IEnumerable<WeatherData> weather;
        if (_location is ZipCode zipCode) {
            weather = await _weatherProvider.GetWeatherAsync(zipCode);
            _weatherForecasts = weather.ToList();
        }
        else if (_location is Address address) {
            weather = await _weatherProvider.GetWeatherAsync(address);
            _weatherForecasts = weather.ToList();
        }

        WeatherForecastsChanged?.Invoke(_weatherForecasts);
        setLoading(false);
    }

    #region Loading
    /// <summary>
    /// ONLY IF NOT LOADED EARLIER
    /// <br/> Loads the data from the persistent storage and fetches weather
    /// <br/> Invokes events
    /// </summary>
    /// <returns></returns>
    public async Task Load() {
        try {
            await _initializeTask.Value;
        }
        catch (Exception) {
            _initializeTask = new(initialize);
            setLoading(false);
            throw;
        }
    }

    private async Task initialize() {
        setLoading(true);
        _location = GetPersistentData()?.Location;
        LocationChanged?.Invoke(_location);

        await FetchWeather();
        setLoading(false);
    }

    private PersistentData? GetPersistentData() {
        try {
            return _dataManager.GetPersistentData();
        }
        catch {
            return null;
        }
    }

    private void setLoading(bool value) {
        if (IsLoading == value) return;

        IsLoading = value;
        LoadingChanged?.Invoke(value);
    }

    #endregion
}
