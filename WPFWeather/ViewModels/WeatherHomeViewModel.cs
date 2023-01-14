using System.Collections.Generic;
using System.Collections.ObjectModel;

using WPFWeather.Models;
using WPFWeather.Models.LocationInfo;
using WPFWeather.Stores;

namespace WPFWeather.ViewModels;
public class WeatherHomeViewModel : ViewModelBase {
    private readonly AppStore _appStore;

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

    public bool IsLoading => _appStore.IsLoading;

    private WeatherData? _selectedWeather = null;
    public WeatherData? SelectedWeather {
        get => _selectedWeather;
        set {
            if (value == _selectedWeather) return;

            _selectedWeather = value;
            OnPropertyChanged(nameof(SelectedWeather));
        }
    }

    public WeatherHomeViewModel(AppStore appStore) {
        _appStore = appStore;

        _location = _appStore.Location?.ToString();
        _appStore.LocationChanged += OnLocationUpdate;

        _appStore.WeatherForecastsChanged += OnWeatherUpdate;

        _appStore.LoadingChanged += OnLoadingUpdate;

        _appStore.Load();
    }

    private void OnWeatherUpdate(IEnumerable<WeatherData> data) {
        Weather.Clear();
        foreach (WeatherData weather in _appStore.WeatherForecasts) {
            Weather.Add(weather);
        }

        if (Weather.Count is > 0 && SelectedWeather is null) {
            SelectedWeather = Weather[0];
        }
    }

    private void OnLocationUpdate(Location? location) {
        Location = location?.ToString();
    }

    private void OnLoadingUpdate(bool isLoading) {
        OnPropertyChanged(nameof(IsLoading));
    }

    public override void Dispose() {
        _appStore.LocationChanged -= OnLocationUpdate;
        _appStore.WeatherForecastsChanged -= OnWeatherUpdate;
    }
}
