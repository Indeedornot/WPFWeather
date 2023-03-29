using System;

namespace WPFWeather.Exceptions.PersistentData;
public class PersistentDataReadException : Exception
{
    public PersistentDataReadException() { }
    public PersistentDataReadException(string message) : base(message) { }
    public PersistentDataReadException(string message, Exception innerException) : base(message, innerException) { }
}