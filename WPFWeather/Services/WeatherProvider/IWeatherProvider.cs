using System.Collections.Generic;
using System.Threading.Tasks;

using WPFWeather.Models;
using WPFWeather.Models.LocationInfo;

namespace WPFWeather.Services.WeatherProvider;
public interface IWeatherProvider {
    public Task<IEnumerable<WeatherData>> GetWeatherAsync(Location location);

    public Task<bool> ValidateLocation(Location location);

    public Task<Location?> GetLocationByAddress(Address address);
    public Task<Location?> GetLocationByZipCode(ZipCode zipCode);
}
