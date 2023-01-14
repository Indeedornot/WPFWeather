using System.Text.Json.Serialization;

namespace WPFWeather.Models.LocationInfo;

[JsonDerivedType(typeof(ZipCode), "ZipCode")]
public class ZipCode : Location {
    public string PostalCode { get; internal set; }
    public string CountryCode { get; internal set; }

    public ZipCode(string postalCode, string countryCode) {
        PostalCode = postalCode;
        CountryCode = countryCode;
    }

    public override string ToString() {
        return $"{PostalCode} {CountryCode}";
    }
}
