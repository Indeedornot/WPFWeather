using System.Text.Json.Serialization;

namespace WPFWeather.Models.LocationInfo;

[JsonDerivedType(typeof(Address), "Address")]
public class Address : Location {
    public string CityName { get; }

    public Address(string cityName) {
        CityName = cityName;
    }

    public override string ToString() {
        return CityName;
    }
}
