using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using WPFWeather.Models;
using WPFWeather.Models.LocationInfo;
using WPFWeather.Services.Provider;
using WPFWeather.Services.WeatherProvider;

namespace WPFWeather.Stores;

public class AppStore
{
    private List<WeatherData> _weatherForecasts;
    public IEnumerable<WeatherData> WeatherForecasts => _weatherForecasts;

    private Location? _location;
    public Location? Location
    {
        get => _location;
        private set
        {
            if (_location == value) return;

            _location = value;
            LocationChanged?.Invoke(value);
        }
    }

    private bool _isLoading;
    /// <summary>
    /// Signals fetching of initial data or refetching weatherForecasts
    /// </summary>
    public bool IsLoading
    {
        get => _isLoading;
        private set
        {
            if (_isLoading == value) return;

            _isLoading = value;
            LoadingChanged?.Invoke(value);
        }
    }

    public DateTime LastFetched { get; private set; }
    public DateTime OldestFetched { get; private set; }
    public DateTime LatestFetched { get; private set; }

    public event Action<Location?> LocationChanged;
    public event Action<IEnumerable<WeatherData>> WeatherForecastsChanged;
    public event Action<bool> LoadingChanged;

    private readonly IWeatherProvider _weatherProvider;
    private readonly IPersistentDataManager _dataManager;
    public AppStore(IWeatherProvider weatherProvider, IPersistentDataManager dataManager)
    {
        _weatherProvider = weatherProvider;
        _dataManager = dataManager;

        _weatherForecasts = new();
        _initializeTask = new(Initialize);
    }

    public void SetLocation(Location? location)
    {
        Location = location;
    }

    public void SetWeather(IEnumerable<WeatherData> weather)
    {
        if (weather is null)
        {
            _weatherForecasts.Clear();
            WeatherForecastsChanged?.Invoke(_weatherForecasts);
            return;
        }

        _weatherForecasts = weather.ToList();
        LastFetched = DateTime.Now;
        LatestFetched = _weatherForecasts.Last().Time;
        OldestFetched = _weatherForecasts.First().Time;

        WeatherForecastsChanged?.Invoke(_weatherForecasts);
    }

    public void AddPastWeather(IEnumerable<WeatherData> weather)
    {
        if (weather is null) return;

        var weatherList = weather.ToList();
        _weatherForecasts.InsertRange(0, weatherList);
        OldestFetched = weatherList.First().Time;
        LastFetched = DateTime.Now;

        WeatherForecastsChanged?.Invoke(_weatherForecasts);
    }

    public void AddFutureWeather(IEnumerable<WeatherData> weather)
    {
        if (weather is null) return;

        var weatherList = weather.ToList();
        _weatherForecasts.AddRange(weatherList);
        LatestFetched = weatherList.Last().Time;
        LastFetched = DateTime.Now;

        WeatherForecastsChanged?.Invoke(_weatherForecasts);
    }

    /// <summary>
    /// Fetches Weather from Now for 3 Days
    /// <br/> Clears all previous weather data if successful
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.Net.Http.HttpRequestException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public async Task ResetWeather(CancellationToken? cancellationToken = null)
    {
        if (Location == null)
        {
            throw new ArgumentException("Location is null");
        }

        IsLoading = true;

        DateTime lastFetched, oldestFetched, latestFetched;
        lastFetched = oldestFetched = DateTime.Now;
        latestFetched = DateTime.Now.AddDays(3);

        try
        {
            IEnumerable<WeatherData> weather = await _weatherProvider.GetWeatherAsync(Location, oldestFetched, latestFetched, cancellationToken);

            LastFetched = lastFetched;
            LatestFetched = latestFetched;
            OldestFetched = oldestFetched;

            _weatherForecasts = weather.ToList();
            WeatherForecastsChanged?.Invoke(_weatherForecasts);
            IsLoading = false;
        }
        catch (Exception e)
        {
            IsLoading = false;
            Debug.WriteLine("Error while fetching weather: ", e.Message);
            throw;
        }
    }

    #region Loading
    /// <summary>
    /// <br/> Initialy loads the data from the persistent storage and fetches weather
    /// </summary>
    /// <returns></returns>
    public async Task Load()
    {
        try
        {
            await _initializeTask.Value;
        }
        catch (Exception)
        {
            _initializeTask = new(Initialize);
            IsLoading = false;
        }
    }

    private Lazy<Task> _initializeTask;
    public bool IsInitialized => _initializeTask.IsValueCreated;

    private async Task Initialize()
    {
        IsLoading = true;

        PersistentData? persistentData = GetPersistentData();
        Location = persistentData?.Location;

        if (!WeatherForecasts.Any() && Location != null)
        {
            await ResetWeather();
        }

        IsLoading = false;
    }

    private PersistentData? GetPersistentData()
    {
        try
        {
            return _dataManager.GetPersistentData();
        }
        catch
        {
            return null;
        }
    }

    #endregion
}