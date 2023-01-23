using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WPFWeather.Models.LocationInfo;

namespace WPFWeather.Services.LocationProvider;
public interface ILocationProvider {
    public Task<Location?> GetLocationByZipCodeAsync(ZipCode zipCode);
    public Task<Location?> GetLocationByAddressAsync(Address address);
}
