using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using WPFWeather.Models.LocationInfo;

namespace WPFWeather.Services.LocationProvider;
internal class GeocodeLocationProvider : ILocationProvider {
    private readonly HttpClient httpClient;

    public GeocodeLocationProvider() {
        httpClient = new HttpClient();
    }

    //TODO: Think about error handling
    public async Task<Location?> GetLocationByAddressAsync(Address address) {
        string requestUri = CreateRequestUri(city: address.CityName);
        var response = await httpClient.GetFromJsonAsync<List<GeocodeLocationModel>>(requestUri);

        GeocodeLocationModel? location = response?.FirstOrDefault();
        if (location == null) return null;

        return GeocodeModelToLocation(location);
    }

    public async Task<Location?> GetLocationByZipCodeAsync(ZipCode zipCode) {
        string requestUri = CreateRequestUri(postalcode: zipCode.PostalCode, country: zipCode.CountryCode);
        var response = await httpClient.GetFromJsonAsync<List<GeocodeLocationModel>>(requestUri);

        GeocodeLocationModel? location = response?.FirstOrDefault();
        if (location == null) return null;

        return GeocodeModelToLocation(location);
    }

    private static string CreateRequestUri(string housenumber = "", string streetname = "", string city = "", string county = "", string state = "", string country = "", string postalcode = "") {
        var uri = new StringBuilder();
        uri.Append("https://geocode.maps.co/search?");
        uri.Append("housenumber=" + housenumber);
        uri.Append("&street=" + housenumber + " " + streetname);
        uri.Append("&city=" + city);
        uri.Append("&county=" + county);
        uri.Append("&state=" + state);
        uri.Append("&country=" + country);
        uri.Append("&postalcode=" + postalcode);

        return uri.ToString();
    }

    private static Location GeocodeModelToLocation(GeocodeLocationModel model) {
        return new Location() {
            CityName = model.DisplayName.Split(",")[0],
            Latitude = double.Parse(model.Lat),
            Longitude = double.Parse(model.Lon)
        };
    }
}
