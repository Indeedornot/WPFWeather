using System;

namespace WPFWeather.Exceptions.PersistentData;
internal class PersistentDataSaveException : Exception {
    public PersistentDataSaveException() { }
    public PersistentDataSaveException(string message) : base(message) { }
    public PersistentDataSaveException(string message, Exception innerException) : base(message, innerException) { }
}
