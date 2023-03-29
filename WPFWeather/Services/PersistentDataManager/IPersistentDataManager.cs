using WPFWeather.Models;

namespace WPFWeather.Services.Provider;
public interface IPersistentDataManager
{
    public PersistentData? GetPersistentData();

    public void SaveData(PersistentData data);
}