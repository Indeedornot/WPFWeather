using System;

using WPFWeather.Models.LocationInfo;

namespace WPFWeather.Exceptions.Location;

public class InvalidZipCodeException : Exception {
    public ZipCode ZipCode;

    public InvalidZipCodeException(ZipCode zipCode) {
        ZipCode = zipCode;
    }

    public InvalidZipCodeException(string message, ZipCode zipCode) : base(message) {
        ZipCode = zipCode;
    }

    public InvalidZipCodeException(string message, Exception innerException, ZipCode zipCode) : base(message, innerException) {
        ZipCode = zipCode;
    }
}