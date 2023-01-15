using System.Collections.Generic;
using System.Threading.Tasks;

using WPFWeather.Models;
using WPFWeather.Models.LocationInfo;

namespace WPFWeather.Services.WeatherProvider;
public interface IWeatherProvider {
    public Task<IEnumerable<WeatherData>> GetWeatherAsync(ZipCode address);
    public Task<IEnumerable<WeatherData>> GetWeatherAsync(Address address);

    public Task<bool> ValidateAddressAsync(Address address);
    public Task<bool> ValidateZipCodeAsync(ZipCode zipCode);
}
