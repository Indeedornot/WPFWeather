using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using WPFWeather.Commands;
using WPFWeather.Models;
using WPFWeather.Models.LocationInfo;
using WPFWeather.Services.NavigationService;
using WPFWeather.Stores;
using WPFWeather.ViewModels.SetLocation;

namespace WPFWeather.ViewModels;
public class WeatherHomeViewModel : ViewModelBase {
    private readonly AppStore _appStore;

    public ObservableCollection<WeatherData> Weather { get; set; }

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

    private string _errorMessage = string.Empty;
    public string ErrorMessage {
        get => _errorMessage;
        set {
            _errorMessage = value;
            OnPropertyChanged(nameof(ErrorMessage));
            OnPropertyChanged(nameof(HasError));
        }
    }

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    private WeatherData? _selectedWeather = null;
    public WeatherData? SelectedWeather {
        get => _selectedWeather;
        set {
            if (value == _selectedWeather) return;

            _selectedWeather = value;
            OnPropertyChanged(nameof(SelectedWeather));
        }
    }

    public LoadWeatherCommand ReloadCommand { get; }
    public NavigateCommand<SetLocationViewModel> SetLocationCommand { get; }

    public WeatherHomeViewModel(AppStore appStore, NavigationService<SetLocationViewModel> setLocationNavigationService) {
        _appStore = appStore;

        _location = _appStore.Location?.ToString();
        Weather = new();

        _appStore.LocationChanged += OnLocationUpdate;
        _appStore.WeatherForecastsChanged += OnWeatherUpdate;
        _appStore.LoadingChanged += OnLoadingUpdate;

        EnsureDataLoaded();

        ReloadCommand = new LoadWeatherCommand(_appStore);
        SetLocationCommand = new NavigateCommand<SetLocationViewModel>(setLocationNavigationService);
    }

    public async Task EnsureDataLoaded() {
        try {
            if (!_appStore.IsInitialized) {
                await _appStore.Load();
                return;
            }

            OnWeatherUpdate(_appStore.WeatherForecasts);
            OnLocationUpdate(_appStore.Location);
            OnLoadingUpdate(_appStore.IsLoading);
        }
        catch (Exception e) {
            ErrorMessage = "Error Loading Forecasts";
            Debug.WriteLine("Error in WeatherHomeViewModel while loading AppStore", e.Message);
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

        if (isLoading) {
            ErrorMessage = string.Empty;
        }
    }

    public override void Dispose() {
        _appStore.LocationChanged -= OnLocationUpdate;
        _appStore.WeatherForecastsChanged -= OnWeatherUpdate;
        _appStore.LoadingChanged -= OnLoadingUpdate;
    }
}
