using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using WPFWeather.Commands;
using WPFWeather.Models;
using WPFWeather.Models.LocationInfo;
using WPFWeather.Stores;

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

    public WeatherHomeViewModel(AppStore appStore) {
        _appStore = appStore;

        _location = _appStore.Location?.ToString();
        Weather = new();

        _appStore.LocationChanged += OnLocationUpdate;
        _appStore.WeatherForecastsChanged += OnWeatherUpdate;
        _appStore.LoadingChanged += OnLoadingUpdate;

        if (!_appStore.IsLoaded) { loadData(); }
        else {
            Weather = new(_appStore.WeatherForecasts);
            Location = _appStore.Location is not null ? _appStore.Location.ToString() : string.Empty;
            IsLoading = _appStore.IsLoading;
        }

        ReloadCommand = new LoadWeatherCommand(this, _appStore);
    }

    public async Task loadData() {
        //Maybe add try/catch in case loading fails and do infinite loading
        try {
            await _appStore.Load();
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
