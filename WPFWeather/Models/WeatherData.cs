using System;

namespace WPFWeather.Models;

public class WeatherData {
    public int Temperature { get; internal set; }
    public string? Description { get; internal set; }
    public int Pressure { get; internal set; }
    public int Humidity { get; internal set; }
    public int WindSpeed { get; internal set; }
    public int Cloudiness { get; internal set; }
    public bool Rain { get; internal set; }
    public bool Snow { get; internal set; }
    public DateTime Time { get; internal set; }
    /// <summary>
    /// The Time the object was fetched from the API
    /// </summary>
    public DateTime FetchedTime { get; internal set; }

    public override bool Equals(object? obj) {
        return obj is WeatherData data &&
            data.Cloudiness == Cloudiness &&
            data.Description == Description &&
            data.FetchedTime == FetchedTime &&
            data.Humidity == Humidity &&
            data.Pressure == Pressure &&
            data.Rain == Rain &&
            data.Snow == Snow &&
            data.Temperature == Temperature &&
            data.WindSpeed == WindSpeed;
    }

    public static bool operator ==(WeatherData? left, WeatherData? right) {
        if (left is null && right is null) return true;
        return left is not null && left.Equals(right);
    }

    public static bool operator !=(WeatherData left, WeatherData right) {
        return !(left == right);
    }
}
