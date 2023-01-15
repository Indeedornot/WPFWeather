namespace WPFWeather.Models.LocationInfo;

public class Location {
    public string CityName { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public override string ToString() {
        return CityName;
    }
}
