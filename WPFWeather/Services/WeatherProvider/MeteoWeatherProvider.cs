using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using WPFWeather.Models;
using WPFWeather.Models.LocationInfo;

namespace WPFWeather.Services.WeatherProvider;
internal class MeteoWeatherProvider : IWeatherProvider {
    private readonly HttpClient httpClient;

    public MeteoWeatherProvider() {
        httpClient = new HttpClient();
    }

    public async Task<IEnumerable<WeatherData>> GetWeatherAsync(Location location, DateTime from, DateTime to) {
        string requestUri = CreateRequestUri(location, from, to);
        MeteoWeatherModel response = await httpClient.GetFromJsonAsync<MeteoWeatherModel>(requestUri);
        //TODO: Errors
        return MeteoModelToWeatherData(response);
    }

    public string CreateRequestUri(Location location, DateTime from, DateTime to) {
        var builder = new StringBuilder();
        builder.Append("https://api.open-meteo.com/v1/forecast?");
        builder.Append($"latitude={location.Latitude.ToString(CultureInfo.InvariantCulture)}");
        builder.Append($"&longitude={location.Longitude.ToString(CultureInfo.InvariantCulture)}");

        builder.Append("&hourly=temperature_2m");
        builder.Append(",relativehumidity_2m");
        builder.Append(",apparent_temperature");
        builder.Append(",precipitation");
        builder.Append(",rain");
        builder.Append(",snowfall");
        builder.Append(",surface_pressure");
        builder.Append(",cloudcover");
        builder.Append(",windspeed_10m");
        builder.Append(",temperature_80m");

        builder.Append("&models=best_match");
        builder.Append($"&start_date={from.ToString("yyyy-MM-dd")}");
        builder.Append($"&end_date={to.ToString("yyyy-MM-dd")}");
        Debug.WriteLine(builder.ToString());
        return builder.ToString();
    }

    public static IEnumerable<WeatherData> MeteoModelToWeatherData(MeteoWeatherModel model) {
        var weatherData = new List<WeatherData>();

        for (int i = 0; i < model.Hourly.Temperature.Count; i++) {
            weatherData.Add(new WeatherData() {
                Temperature = model.Hourly.Temperature[i],
                Time = DateTime.Parse(model.Hourly.Time[i]),
                WindSpeed = model.Hourly.Windspeed[i],
                Humidity = model.Hourly.Relativehumidity[i],
                Rain = model.Hourly.Rain[i],
                Snowfall = model.Hourly.Snowfall[i],
                Cloudiness = model.Hourly.Cloudcover[i],
                Pressure = model.Hourly.SurfacePressure[i],
            });
        }

        return weatherData;
    }
}
