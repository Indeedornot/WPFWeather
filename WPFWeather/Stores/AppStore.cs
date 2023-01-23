using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public Location? Location {
        get => _location;
        private set {
            if (_location == value) return;

            _location = value;
            LocationChanged?.Invoke(value);
        }
    }

    private bool _isLoading;
    /// <summary>
    /// Signals fetching of initial data or refetching weatherForecasts
    /// </summary>
    public bool IsLoading {
        get => _isLoading;
        private set {
            if (_isLoading == value) return;

            _isLoading = value;
            LoadingChanged?.Invoke(value);
        }
    }

    private bool _isFetching;
    /// <summary>
    /// Signals fetching of additional weatherForecasts
    /// </summary>
    public bool IsFetching {
        get => _isFetching;
        private set {
            if (_isFetching == value) return;

            _isFetching = value;
            FetchingChanged?.Invoke(value);
        }
    }

    public DateTime OldestFetched { get; internal set; }
    public DateTime LatestFetched { get; internal set; }
    public DateTime LastFetched { get; internal set; }

    public event Action<Location?> LocationChanged;
    public event Action<IEnumerable<WeatherData>> WeatherForecastsChanged;
    public event Action<bool> LoadingChanged;
    public event Action<bool> FetchingChanged;

    private readonly IWeatherProvider _weatherProvider;
    private readonly IPersistentDataManager _dataManager;
    public AppStore(IWeatherProvider weatherProvider, IPersistentDataManager dataManager) {
        _weatherProvider = weatherProvider;
        _dataManager = dataManager;

        _weatherForecasts = new();
        _initializeTask = new(Initialize);
    }

    public void SetLocation(Location? location) {
        Location = location;
        ResetWeather();
    }

    /// <summary>
    /// Fetches Weather from Now for 3 Days
    /// <br/> Clears all previous weather data
    /// </summary>
    /// <returns></returns>
    public async Task ResetWeather() {
        if (Location == null) return;
        IsLoading = true;

        DateTime lastFetched, oldestFetched, latestFetched;
        lastFetched = oldestFetched = DateTime.Now;
        latestFetched = DateTime.Now.AddDays(3);

        try {
            IEnumerable<WeatherData> weather = await _weatherProvider.GetWeatherAsync(Location, oldestFetched, latestFetched);

            LastFetched = lastFetched;
            LatestFetched = latestFetched;
            OldestFetched = oldestFetched;

            _weatherForecasts = weather.ToList();
            WeatherForecastsChanged?.Invoke(_weatherForecasts);
        }
        catch (Exception e) {
            Debug.WriteLine("Error while fetching weather: ", e.Message);
        }

        IsLoading = false;
    }

    public async Task FetchFutureWeather() {
        if (Location == null) return;
        if (IsFetching || IsLoading) return;

        DateTime latestFetched = DateTime.Now.AddDays(3);
        if (latestFetched > DateTime.Now.AddDays(12)) return;

        IsFetching = true;

        try {
            IEnumerable<WeatherData> weather = await _weatherProvider.GetWeatherAsync(Location, LatestFetched, latestFetched);

            LastFetched = DateTime.Now;
            LatestFetched = latestFetched;

            _weatherForecasts.AddRange(weather);
            WeatherForecastsChanged?.Invoke(_weatherForecasts);
        }
        catch (Exception e) {
            Debug.WriteLine("Error while fetching future weather: ", e.Message);
        }

        IsFetching = false;
    }

    public async Task FetchPastWeather() {
        if (Location == null) return;
        if (IsFetching || IsLoading) return;

        DateTime oldestFetched = DateTime.Now.Subtract(TimeSpan.FromDays(3));
        if (oldestFetched < DateTime.Now.Subtract(TimeSpan.FromDays(12))) return;

        IsFetching = true;

        try {
            IEnumerable<WeatherData> weather = await _weatherProvider.GetWeatherAsync(Location, oldestFetched, OldestFetched);

            LastFetched = DateTime.Now;
            OldestFetched = oldestFetched;

            _weatherForecasts.InsertRange(0, weather);
            WeatherForecastsChanged?.Invoke(_weatherForecasts);
        }
        catch (Exception e) {
            Debug.WriteLine("Error while fetching future weather: ", e.Message);
        }

        IsFetching = false;
    }

    #region Loading
    /// <summary>
    /// <br/> Initialy loads the data from the persistent storage and fetches weather
    /// </summary>
    /// <returns></returns>
    public async Task Load() {
        try {
            await _initializeTask.Value;
        }
        catch (Exception) {
            _initializeTask = new(Initialize);
            IsLoading = false;
            throw;
        }
    }

    private Lazy<Task> _initializeTask;
    public bool IsInitialized => _initializeTask.IsValueCreated;

    private async Task Initialize() {
        IsLoading = true;

        PersistentData? persistentData = GetPersistentData();
        Location = persistentData?.Location;

        if (!WeatherForecasts.Any() && Location != null) {
            await ResetWeather();
        }

        IsLoading = false;
    }

    private PersistentData? GetPersistentData() {
        try {
            return _dataManager.GetPersistentData();
        }
        catch {
            return null;
        }
    }

    #endregion
}
