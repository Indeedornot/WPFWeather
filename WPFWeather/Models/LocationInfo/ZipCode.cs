namespace WPFWeather.Models.LocationInfo;

public class ZipCode
{
    public string PostalCode { get; internal set; }
    public string CountryCode { get; internal set; }

    public ZipCode(string postalCode, string countryCode)
    {
        PostalCode = postalCode;
        CountryCode = countryCode;
    }

    public override string ToString()
    {
        return $"{PostalCode} {CountryCode}";
    }
}