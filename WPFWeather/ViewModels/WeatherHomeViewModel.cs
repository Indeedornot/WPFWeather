using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using WPFWeather.Commands;
using WPFWeather.Models;
using WPFWeather.Models.LocationInfo;
using WPFWeather.Services.NavigationService;
using WPFWeather.Services.WeatherProvider;
using WPFWeather.Stores;
using WPFWeather.ViewModels.SetLocation;

namespace WPFWeather.ViewModels;
public class WeatherHomeViewModel : ViewModelBase {
    private readonly AppStore _appStore;
    private readonly IWeatherProvider _weatherProvider;

    public ObservableCollection<WeatherData> Weather { get; set; } = new();

    private string? _location;
    public string? Location {
        get => _location;
        set {
            if (value == _location) return;

            _location = value;
            OnPropertyChanged(nameof(Location));
        }
    }

    private bool _isLoading;
    public bool IsLoading {
        get => _isLoading;
        set {
            if (_isLoading == value) return;

            _isLoading = value;
            OnPropertyChanged(nameof(IsLoading));
        }
    }

    private bool _isFetching;
    public bool IsFetching {
        get => _isFetching;
        set {
            if (_isFetching == value) return;

            _isFetching = value;
            OnPropertyChanged(nameof(IsFetching));
        }
    }

    private string? _errorMessage = string.Empty;
    public string? ErrorMessage {
        get => _errorMessage;
        set {
            _errorMessage = value;
            OnPropertyChanged(nameof(ErrorMessage));
            OnPropertyChanged(nameof(HasError));
        }
    }

    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    private WeatherData? _selectedWeather = null;
    public WeatherData? SelectedWeather {
        get => _selectedWeather;
        set {
            if (value == _selectedWeather) return;

            _selectedWeather = value;
            OnPropertyChanged(nameof(SelectedWeather));
        }
    }

    public CancellationTokenSource TokenSource { get; } = new();

    public ReloadWeatherCommand ReloadCommand { get; }
    public RelayCommand FetchFutureWeatherCommand { get; }
    public RelayCommand FetchPastWeatherCommand { get; }
    public NavigateCommand<SetLocationViewModel> SetLocationCommand { get; }

    public WeatherHomeViewModel(AppStore appStore, IWeatherProvider weatherProvider, NavigationService<SetLocationViewModel> setLocationNavigationService) {
        _appStore = appStore;
        _weatherProvider = weatherProvider;

        _appStore.LocationChanged += OnLocationUpdate;
        _appStore.WeatherForecastsChanged += OnWeatherUpdate;

        EnsureDataLoaded();

        SelectedWeather = Weather?.FirstOrDefault();

        ReloadCommand = new ReloadWeatherCommand(this, _appStore);
        SetLocationCommand = new NavigateCommand<SetLocationViewModel>(setLocationNavigationService);
        FetchFutureWeatherCommand = new RelayCommand((_) => FetchFutureWeather());
        FetchPastWeatherCommand = new RelayCommand((_) => FetchPastWeather());
    }

    public async Task FetchFutureWeather() {
        Location? location = _appStore.Location;
        DateTime fetchFrom = _appStore.LatestFetched;

        if (location == null) return;
        if (IsFetching || IsLoading) return;

        DateTime fetchTo = fetchFrom.AddDays(3);
        if (fetchTo > DateTime.Now.AddDays(12)) return;
        IsFetching = true;

        try {
            IEnumerable<WeatherData> weather = await _weatherProvider.GetWeatherAsync(location, fetchFrom, fetchTo, TokenSource.Token);
            _appStore.AddFutureWeather(weather);

            ErrorMessage = null;
        }
        catch (Exception e) {
            Debug.WriteLine("Error while fetching future weather: ", e.Message);
            ErrorMessage = "Error while fetching future weather";
        }

        IsFetching = false;
    }

    public async Task FetchPastWeather() {
        Location? location = _appStore.Location;
        DateTime fetchTo = _appStore.OldestFetched;

        if (location == null) return;
        if (IsFetching || IsLoading) return;

        DateTime fetchFrom = fetchTo.Subtract(TimeSpan.FromDays(3));
        if (fetchFrom < DateTime.Now.Subtract(TimeSpan.FromDays(12))) return;
        IsFetching = true;

        try {
            IEnumerable<WeatherData> weather = await _weatherProvider.GetWeatherAsync(location, fetchFrom, fetchTo, TokenSource.Token);
            _appStore.AddPastWeather(weather);

            ErrorMessage = null;
        }
        catch (Exception e) {
            Debug.WriteLine("Error while fetching future weather: ", e.Message);
            ErrorMessage = "Error while fetching past weather";
        }

        IsFetching = false;
    }


    public async Task EnsureDataLoaded() {
        Weather = new(_appStore.WeatherForecasts);
        Location = _appStore.Location?.ToString();
        IsLoading = _appStore.IsLoading;

        if (!_appStore.IsInitialized) {
            await _appStore.Load();
            //error message
        }
    }

    private void OnWeatherUpdate(IEnumerable<WeatherData> data) {
        Weather.Clear();
        foreach (WeatherData weather in _appStore.WeatherForecasts) {
            Weather.Add(weather);
        }

        //Add case when old selected weather is no longer available
        if (Weather.Count is > 0 && SelectedWeather is null) {
            SelectedWeather = Weather[0];
        }
    }
    private void OnLocationUpdate(Location? location) {
        Location = location?.ToString();
    }
    private void OnLoadingUpdate(bool isLoading) {
        IsLoading = isLoading;
    }

    public override void Dispose() {
        _appStore.LocationChanged -= OnLocationUpdate;
        _appStore.WeatherForecastsChanged -= OnWeatherUpdate;
        _appStore.LoadingChanged -= OnLoadingUpdate;
    }
}
