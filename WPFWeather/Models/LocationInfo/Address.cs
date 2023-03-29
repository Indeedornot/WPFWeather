namespace WPFWeather.Models.LocationInfo;

public class Address
{
    public string CityName { get; }

    public Address(string cityName)
    {
        CityName = cityName;
    }

    public override string ToString()
    {
        return CityName;
    }
}