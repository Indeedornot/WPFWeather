using System;

using WPFWeather.Models.LocationInfo;

namespace WPFWeather.Exceptions.Location;

public class InvalidAddressException : Exception {
    public Address Address;

    public InvalidAddressException(Address address) {
        Address = address;
    }

    public InvalidAddressException(string message, Address address) : base(message) {
        Address = address;
    }

    public InvalidAddressException(string message, Exception innerException, Address address) : base(message, innerException) {
        Address = address;
    }
}