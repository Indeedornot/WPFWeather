using System;
using System.IO;
using System.Text.Json;

using WPFWeather.Exceptions.PersistentData;
using WPFWeather.Models;
using WPFWeather.Services.Provider;

namespace WPFWeather.Services.DataProvider;
public class JsonPersistentDataManager : IPersistentDataManager
{
    private readonly string _storageFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private readonly string _storageFolderName = "WPFWeather";
    private string storageFolderPath => Path.Combine(_storageFolderPath, _storageFolderName);

    private readonly string _storageFileName = "persistent_data";
    private readonly string _storageFileExtension = "json";
    private string storageFilePath => Path.Combine(storageFolderPath, $"{_storageFileName}.{_storageFileExtension}");

    public PersistentData? GetPersistentData()
    {
        try
        {
            string data = File.ReadAllText(storageFilePath);
            //TODO: Location does not fail on null for CityName
            return JsonSerializer.Deserialize<PersistentData>(data);
        }
        catch (Exception e)
        {
            HandleReadError(e);
            throw new PersistentDataReadException("Error reading persistent data", e);
        }
    }

    public void SaveData(PersistentData data)
    {
        try
        {
            EnsureDataFolderExists();
            string json = JsonSerializer.Serialize(data);
            File.WriteAllText(storageFilePath, json);
        }
        catch (Exception e)
        {
            throw new PersistentDataSaveException("Error saving persistent data", e);
        }
    }

    private void EnsureDataFolderExists()
    {
        string folderPath = Path.Combine(_storageFolderPath, _storageFolderName);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }

    private void HandleReadError(Exception e)
    {
        if (e is DirectoryNotFoundException)
        {
            EnsureDataFolderExists();
        }

        if (e is JsonException)
        {
            File.Move(storageFilePath, Path.Combine(storageFolderPath, $"{_storageFileName}.{DateTime.Now.Ticks}.{_storageFileExtension}"));
        }
    }
}