using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using WPFWeather.Models;
using WPFWeather.Models.LocationInfo;

namespace WPFWeather.Services.WeatherProvider;
internal class MeteoWeatherProvider : IWeatherProvider {
    private readonly HttpClient httpClient;

    public MeteoWeatherProvider() {
        httpClient = new HttpClient();
    }

    public async Task<IEnumerable<WeatherData>> GetWeatherAsync(Location location, DateTime from, DateTime to, CancellationToken? cancellationToken) {
        string requestUri = CreateRequestUri(location, from, to);
        MeteoWeatherModel response = await httpClient.GetFromJsonAsync<MeteoWeatherModel>(requestUri, cancellationToken: cancellationToken ?? CancellationToken.None);
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
        builder.Append(",rain");
        builder.Append(",snowfall");
        builder.Append(",surface_pressure");
        builder.Append(",cloudcover");
        builder.Append(",windspeed_10m");
        builder.Append(",weathercode");

        builder.Append("&models=best_match");
        builder.Append($"&start_date={from.ToString("yyyy-MM-dd")}");
        builder.Append($"&end_date={to.ToString("yyyy-MM-dd")}");
        Debug.WriteLine(builder.ToString());
        return builder.ToString();
    }

    private static IEnumerable<WeatherData> MeteoModelToWeatherData(MeteoWeatherModel model) {
        var weatherData = new List<WeatherData>();

        for (int i = 0; i < model.Hourly.Temperature.Count; i++) {
            weatherData.Add(new WeatherData() {
                Temperature = model.Hourly.Temperature[i],
                Time = DateTime.Parse(model.Hourly.Time[i]),
                WindSpeed = model.Hourly.Windspeed[i],
                Humidity = model.Hourly.Relativehumidity[i],
                Rain = model.Hourly.Rain[i],
                Snowfall = model.Hourly.Snowfall[i],
                CloudPercentage = model.Hourly.Cloudcover[i],
                Pressure = model.Hourly.SurfacePressure[i],
                Description = WeatherCodeToWeatherType(model.Hourly.Weathercode[i])
            });
        }

        return weatherData;
    }

    private static WeatherType WeatherCodeToWeatherType(int weatherCode) {
        return weatherCode switch {
            0 => WeatherType.Clear,
            1 or 2 or 3 => WeatherType.PartlyCloudy,
            45 or 48 => WeatherType.Fog,
            51 or 53 or 55 => WeatherType.Drizzle,
            61 or 63 or 65 or 66 or 67 or 80 or 81 or 82 => WeatherType.Rain,
            71 or 73 or 75 or 77 or 85 or 86 => WeatherType.Snowfall,
            95 or 96 or 99 => WeatherType.Thunderstorm,
            _ => WeatherType.Clear
        };
    }
}
