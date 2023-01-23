using System;

namespace WPFWeather.Models;

public class WeatherData {
    public double Temperature { get; internal set; }
    public double Pressure { get; internal set; }
    public double Humidity { get; internal set; }
    public double WindSpeed { get; internal set; }
    public double Cloudiness { get; internal set; }
    public double Rain { get; internal set; }
    public double Snowfall { get; internal set; }
    public DateTime Time { get; internal set; }

    public override bool Equals(object? obj) {
        return obj is WeatherData data &&
            data.Cloudiness == Cloudiness &&
            data.Humidity == Humidity &&
            data.Pressure == Pressure &&
            data.Rain == Rain &&
            data.Snowfall == Snowfall &&
            data.Temperature == Temperature &&
            data.WindSpeed == WindSpeed;
    }

    public static bool operator ==(WeatherData? left, WeatherData? right) {
        if (left is null && right is null) {
            return true;
        }

        return left is not null && left.Equals(right);
    }

    public static bool operator !=(WeatherData left, WeatherData right) {
        return !(left == right);
    }

    public override int GetHashCode() {
        return HashCode.Combine(Temperature, Pressure, Humidity, WindSpeed, Cloudiness, Rain, Snowfall, Time);
    }
}
