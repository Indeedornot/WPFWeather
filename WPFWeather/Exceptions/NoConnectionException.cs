using System;

namespace WPFWeather.Exceptions;
public class NoConnectionException : Exception {
    public NoConnectionException() { }
    public NoConnectionException(string message) : base(message) { }
    public NoConnectionException(string message, Exception inner) : base(message, inner) { }
}
