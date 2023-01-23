using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable S125 // Sections of code should not be commented out

namespace WPFWeather.Services.WeatherProvider;

public class CurrentWeather {
    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }

    [JsonPropertyName("windspeed")]
    public double Windspeed { get; set; }

    [JsonPropertyName("winddirection")]
    public double Winddirection { get; set; }

    [JsonPropertyName("weathercode")]
    public int Weathercode { get; set; }

    [JsonPropertyName("time")]
    public string Time { get; set; }
}

public class Hourly {
    [JsonPropertyName("time")]
    public List<string> Time { get; set; }

    [JsonPropertyName("temperature_2m")]
    public List<double> Temperature { get; set; }

    [JsonPropertyName("relativehumidity_2m")]
    public List<int> Relativehumidity { get; set; }

    //[JsonPropertyName("apparent_temperature")]
    //public List<double> ApparentTemperature { get; set; }

    //[JsonPropertyName("precipitation")]
    //public List<double> Precipitation { get; set; }

    [JsonPropertyName("rain")]
    public List<double> Rain { get; set; }

    [JsonPropertyName("snowfall")]
    public List<double> Snowfall { get; set; }

    [JsonPropertyName("surface_pressure")]
    public List<double> SurfacePressure { get; set; }

    [JsonPropertyName("cloudcover")]
    public List<int> Cloudcover { get; set; }

    [JsonPropertyName("windspeed_10m")]
    public List<double> Windspeed { get; set; }
}

//public class HourlyUnits {
//    [JsonPropertyName("time")]
//    public string Time { get; set; }

//    [JsonPropertyName("temperature_2m")]
//    public string Temperature { get; set; }

//    [JsonPropertyName("relativehumidity_2m")]
//    public string Relativehumidity { get; set; }

//    [JsonPropertyName("apparent_temperature")]
//    public string ApparentTemperature { get; set; }

//    [JsonPropertyName("precipitation")]
//    public string Precipitation { get; set; }

//    [JsonPropertyName("rain")]
//    public string Rain { get; set; }

//    [JsonPropertyName("snowfall")]
//    public string Snowfall { get; set; }

//    [JsonPropertyName("surface_pressure")]
//    public string SurfacePressure { get; set; }

//    [JsonPropertyName("cloudcover")]
//    public string Cloudcover { get; set; }

//    [JsonPropertyName("windspeed_10m")]
//    public string Windspeed { get; set; }
//}

public class MeteoWeatherModel {
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("generationtime_ms")]
    public double GenerationtimeMs { get; set; }

    [JsonPropertyName("utc_offset_seconds")]
    public int UtcOffsetSeconds { get; set; }

    //[JsonPropertyName("timezone")]
    //public string Timezone { get; set; }

    //[JsonPropertyName("timezone_abbreviation")]
    //public string TimezoneAbbreviation { get; set; }

    [JsonPropertyName("elevation")]
    public double Elevation { get; set; }

    //[JsonPropertyName("current_weather")]
    //public CurrentWeather CurrentWeather { get; set; } 

    //[JsonPropertyName("hourly_units")]
    //public HourlyUnits HourlyUnits { get; set; }

    [JsonPropertyName("hourly")]
    public Hourly Hourly { get; set; }
}