using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Weather.NET;
using Weather.NET.Enums;
using Weather.NET.Models.WeatherModel;

using WPFWeather.Exceptions;
using WPFWeather.Models;
using WPFWeather.Models.LocationInfo;

using Location = WPFWeather.Models.LocationInfo.Location;

namespace WPFWeather.Services.WeatherProvider;

public class OpenWeatherService : IWeatherProvider {

    private readonly string NoConnectionError = "No such host is known. (api.openweathermap.org:443)";
    private readonly WeatherClient weatherApi;
    public OpenWeatherService(string ApiKey) {
        weatherApi = new WeatherClient(ApiKey);
    }

    /// <summary>
    /// Gets the weather forecasts for city by location
    /// </summary>
    /// <param name="address"></param>
    /// <returns>Forecasts for the next 3 days separated each by 3 hours</returns>
    /// <exception cref="InvalidLocationException">Thrown when OpenWeatherApi can't find the address</exception>
    public async Task<IEnumerable<WeatherData>> GetWeatherAsync(Location location) {
        try {
            //Gets forecast, each timestamp is separated by 3h
            List<WeatherModel> forecasts = await weatherApi.GetForecastAsync(latitude: location.Latitude, longitude: location.Longitude, timestampCount: 24, measurement: Measurement.Metric);
            return forecasts.Select(WeatherModelToWeather);
        }
        catch (Exception e) {
            if (e.Message == NoConnectionError) {
                throw new NoConnectionException("No internet connection");
            }

            throw new InvalidLocationException("Invalid Location", e, location);
        }
    }

    private static WeatherData WeatherModelToWeather(WeatherModel model) {
        return new WeatherData() {
            Cloudiness = model.Clouds.Percentage,
            WindSpeed = (int)model.Wind.Speed,
            Temperature = (int)model.Main.Temperature,
            Pressure = model.Main.AtmosphericPressure,
            Humidity = model.Main.HumidityPercentage,
            Description = model.Weather.FirstOrDefault()?.Description ?? null,
            Rain = model.Rain?.PastHourVolume is > 0,
            Snow = model.Snow?.PastHourVolume is > 0,
            FetchedTime = model.AnalysisDate,
            Time = model.AnalysisDate
        };
    }

    public async Task<bool> ValidateLocation(Location location) {
        try {
            Task<WeatherModel> task = weatherApi.GetCurrentWeatherAsync(latitude: location.Latitude, longitude: location.Longitude);
            if (await Task.WhenAny(task, Task.Delay(1000)) == task) {
                return true;
            }

            return false;
        }
        catch {
            return false;
        }
    }

    public async Task<Location?> GetLocationByAddress(Address address) {
        if (string.IsNullOrWhiteSpace(address.CityName)) {
            return null;
        }

        try {
            Task<WeatherModel> task = weatherApi.GetCurrentWeatherAsync(cityName: address.CityName);
            if (await Task.WhenAny(task, Task.Delay(1000)) == task) {
                WeatherModel model = await task;
                return new Location() {
                    CityName = model.CityName,
                    Latitude = model.Location.Latitude,
                    Longitude = model.Location.Longitude
                };
            }

            return null;
        }
        catch {
            return null;
        }
    }

    public async Task<Location?> GetLocationByZipCode(ZipCode zipCode) {
        if (string.IsNullOrWhiteSpace(zipCode.PostalCode) || string.IsNullOrWhiteSpace(zipCode.CountryCode)) {
            return null;
        }

        try {
            Task<WeatherModel> task = weatherApi.GetCurrentWeatherAsync(zipCode: zipCode.PostalCode, countryCode: zipCode.CountryCode);
            if (await Task.WhenAny(task, Task.Delay(1000)) == task) {
                WeatherModel model = await task;
                return new Location() {
                    CityName = model.CityName,
                    Latitude = model.Location.Latitude,
                    Longitude = model.Location.Longitude
                };
            }

            return null;
        }
        catch {
            return null;
        }
    }
}
