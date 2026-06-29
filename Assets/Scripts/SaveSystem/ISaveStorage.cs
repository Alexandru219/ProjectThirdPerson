using System.IO;
using UnityEngine;

public interface ISaveStorage
{
    void Write(string key, string json);
    string Read(string key);
    bool Has(string key);
    void   Delete(string key);
    void DeleteAll();
}


public sealed class PlayerPrefsStorage : ISaveStorage
{
    public void  Write(string key, string json) => PlayerPrefs.SetString(key, json);
    public string Read(string key) => PlayerPrefs.GetString(key);
    public bool Has(string key) => PlayerPrefs.HasKey(key);
    public void Delete(string key) { PlayerPrefs.DeleteKey(key); PlayerPrefs.Save(); }
    public void DeleteAll() { PlayerPrefs.DeleteAll(); PlayerPrefs.Save(); }
}

public sealed class FileStorage : ISaveStorage
{
    private readonly string _dir;

    public FileStorage(string subFolder = "saves")
    {
        _dir = Path.Combine(Application.persistentDataPath, subFolder);
        Directory.CreateDirectory(_dir);
    }

    public void Write(string key, string json)
    {
        string target = FilePath(key);
        string temp = target + ".tmp";
        File.WriteAllText(temp, json);         
        if (File.Exists(target)) File.Delete(target); 
        File.Move(temp, target);                  
    }

    public string Read(string key) => File.ReadAllText(FilePath(key));
    public bool   Has(string key) => File.Exists(FilePath(key));
    public void   Delete(string key) { if (Has(key)) File.Delete(FilePath(key)); }

    public void DeleteAll()
    {
        foreach (string f in Directory.GetFiles(_dir, "*.json"))
            File.Delete(f);
    }

    private string FilePath(string key) => Path.Combine(_dir, key + ".json");
}


public static class SaveStorageFactory
{
    public static ISaveStorage Create()
    {
        return Application.platform switch
        {
            RuntimePlatform.WebGLPlayer => new PlayerPrefsStorage(),
            RuntimePlatform.WindowsPlayer
                or RuntimePlatform.OSXPlayer
                or RuntimePlatform.LinuxPlayer
                or RuntimePlatform.Android
                or RuntimePlatform.IPhonePlayer => new FileStorage(),
            RuntimePlatform.WindowsEditor
                or RuntimePlatform.OSXEditor
                or RuntimePlatform.LinuxEditor  => new FileStorage("saves_editor"),
            _ => new PlayerPrefsStorage()
        };
    }
}
