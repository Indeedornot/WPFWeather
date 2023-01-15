using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Weather.NET;
using Weather.NET.Enums;
using Weather.NET.Models.WeatherModel;

using WPFWeather.Exceptions;
using WPFWeather.Exceptions.Location;
using WPFWeather.Models;
using WPFWeather.Models.LocationInfo;

namespace WPFWeather.Services.WeatherProvider;

public class OpenWeatherService : IWeatherProvider {

    private readonly string NoConnectionError = "No such host is known. (api.openweathermap.org:443)";
    private readonly WeatherClient weatherApi;
    public OpenWeatherService(string ApiKey) {
        weatherApi = new WeatherClient(ApiKey);
    }

    /// <summary>
    /// Gets the weather forecasts for city given by ZipCode data
    /// </summary>
    /// <param name="address"></param>
    /// <returns>Forecasts for the next 3 days separated each by 3 hours</returns>
    /// <exception cref="InvalidZipCodeException">Thrown when OpenWeatherApi can't find the address</exception>
    public async Task<IEnumerable<WeatherData>> GetWeatherAsync(ZipCode address) {
        try {
            //Gets forecast, each timestamp is separated by 3h
            List<WeatherModel> forecasts = await weatherApi.GetForecastAsync(address.PostalCode, address.CountryCode, timestampCount: 24, measurement: Measurement.Metric);
            return forecasts.Select(WeatherModelToWeather);
        }
        catch (Exception e) {
            if (e.Message == NoConnectionError) {
                throw new NoConnectionException("No internet connection");
            }

            throw new InvalidZipCodeException("Invalid ZipCode", e, address);
        }
    }

    /// <summary>
    /// Gets the weather forecasts for city given by Address data
    /// </summary>
    /// <param name="address"></param>
    /// <returns>Forecasts for the next 3 days separated each by 3 hours</returns>
    /// <exception cref="InvalidAddressException">Thrown when OpenWeatherApi can't find the address</exception>
    public async Task<IEnumerable<WeatherData>> GetWeatherAsync(Address address) {
        try {
            //Gets forecast, each timestamp is separated by 3h
            List<WeatherModel> forecasts = await weatherApi.GetForecastAsync(address.CityName, timestampCount: 24, measurement: Measurement.Metric);
            return forecasts.Select(WeatherModelToWeather);
        }
        catch (Exception e) {
            if (e.Message == NoConnectionError) {
                throw new NoConnectionException("No internet connection");
            }

            throw new InvalidAddressException("Invalid Address", e, address);
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

    public async Task<bool> ValidateAddressAsync(Address address) {
        if (string.IsNullOrWhiteSpace(address.CityName)) {
            return false;
        }

        try {
            Task<WeatherModel> task = weatherApi.GetCurrentWeatherAsync(cityName: address.CityName);
            return await Task.WhenAny(task, Task.Delay(1000)) == task;
        }
        catch {
            return false;
        }
    }

    public async Task<bool> ValidateZipCodeAsync(ZipCode zipCode) {
        if (string.IsNullOrWhiteSpace(zipCode.PostalCode) || string.IsNullOrWhiteSpace(zipCode.CountryCode)) {
            return false;
        }

        try {
            Task<WeatherModel> task = weatherApi.GetCurrentWeatherAsync(zipCode: zipCode.PostalCode, countryCode: zipCode.CountryCode);
            return await Task.WhenAny(task, Task.Delay(1000)) == task;
        }
        catch {
            return false;
        }
    }
}
