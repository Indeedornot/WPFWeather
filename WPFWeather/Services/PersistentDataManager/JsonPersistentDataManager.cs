using System;
using System.IO;
using System.Text.Json;

using WPFWeather.Exceptions.PersistentData;
using WPFWeather.Models;
using WPFWeather.Services.Provider;

namespace WPFWeather.Services.DataProvider;
public class JsonPersistentDataManager : IPersistentDataManager {
    private readonly string _storageFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private readonly string _storageFolderName = "WPFWeather";
    private readonly string _storageFileName = "persistent_data.json";

    private string storageFilePath => Path.Combine(_storageFolderPath, _storageFolderName, _storageFileName);

    public PersistentData? GetPersistentData() {
        try {
            string data = File.ReadAllText(storageFilePath);
            return JsonSerializer.Deserialize<PersistentData>(data);
        }
        catch (Exception e) {
            throw new PersistentDataReadException("Error reading persistent data", e);
        }
    }

    public void SaveData(PersistentData data) {
        try {
            EnsureDataFolderExists();
            string json = JsonSerializer.Serialize(data);
            File.WriteAllText(storageFilePath, json);
        }
        catch (Exception e) {
            throw new PersistentDataSaveException("Error saving persistent data", e);
        }
    }

    private void EnsureDataFolderExists() {
        string folderPath = Path.Combine(_storageFolderPath, _storageFolderName);
        if (!Directory.Exists(folderPath)) {
            Directory.CreateDirectory(folderPath);
        }
    }
}
