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
        if (Location == null) {
            _weatherForecasts.Clear();
            WeatherForecastsChanged?.Invoke(_weatherForecasts);
            return;
        }

        setLoading(true);
        IEnumerable<WeatherData> weather = await _weatherProvider.GetWeatherAsync(Location);
        _weatherForecasts = weather.ToList();
        WeatherForecastsChanged?.Invoke(_weatherForecasts);
        setLoading(false);
    }

    #region Loading
    /// <summary>
    /// <b> ONLY SETS PROPERTY IF NOT SET EARLIER </b>
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
        if (Location == null) {
            var persistentData = GetPersistentData();
            _location = persistentData?.Location;
            LocationChanged?.Invoke(_location);
        }

        if (WeatherForecasts.Count() == 0) {
            await FetchWeather();
        }

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
