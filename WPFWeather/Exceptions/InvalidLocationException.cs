using System;

using Location = WPFWeather.Models.LocationInfo.Location;

namespace WPFWeather.Exceptions;

public class InvalidLocationException : Exception
{
    public Location Location;

    public InvalidLocationException(Location location)
    {
        Location = location;
    }

    public InvalidLocationException(string message, Location location) : base(message)
    {
        Location = location;
    }

    public InvalidLocationException(string message, Exception innerException, Location location) : base(message, innerException)
    {
        Location = location;
    }
}