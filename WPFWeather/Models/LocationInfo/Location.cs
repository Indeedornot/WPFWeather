using System.Text.Json.Serialization;

namespace WPFWeather.Models.LocationInfo;

[JsonDerivedType(typeof(ZipCode), "ZipCode")]
[JsonDerivedType(typeof(Address), "Address")]
public class Location {
}
