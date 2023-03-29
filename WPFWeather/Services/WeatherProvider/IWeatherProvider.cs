using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using WPFWeather.Models;
using WPFWeather.Models.LocationInfo;

namespace WPFWeather.Services.WeatherProvider;
public interface IWeatherProvider
{
    /// <summary>
    /// Fetches WeatherData for the given location
    /// </summary>
    /// <param name="location"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public Task<IEnumerable<WeatherData>> GetWeatherAsync(Location location, DateTime from, DateTime to, CancellationToken? cancellationToken = null);
}