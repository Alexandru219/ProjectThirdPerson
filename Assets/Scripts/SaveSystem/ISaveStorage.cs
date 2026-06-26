using System.IO;
using UnityEngine;

/// <summary>
/// Абстракция хранилища. SaveableObject знает только этот интерфейс —
/// никакого PlayerPrefs / File внутри gameplay-кода.
/// </summary>
public interface ISaveStorage
{
    void   Write(string key, string json);
    string Read(string key);
    bool   Has(string key);
    void   Delete(string key);
    void   DeleteAll();
}

// ── PlayerPrefs (WebGL + fallback) ────────────────────────────────────────────

public sealed class PlayerPrefsStorage : ISaveStorage
{
    public void   Write(string key, string json) => PlayerPrefs.SetString(key, json);
    public string Read(string key)               => PlayerPrefs.GetString(key);
    public bool   Has(string key)                => PlayerPrefs.HasKey(key);
    public void   Delete(string key)             { PlayerPrefs.DeleteKey(key); PlayerPrefs.Save(); }
    public void   DeleteAll()                    { PlayerPrefs.DeleteAll();    PlayerPrefs.Save(); }
}

// ── File system (Standalone / Android / iOS) ──────────────────────────────────

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
        string temp   = target + ".tmp";
        File.WriteAllText(temp, json);          // атомарная запись:
        if (File.Exists(target)) File.Delete(target); // сначала temp,
        File.Move(temp, target);                      // потом переименование
    }

    public string Read(string key)   => File.ReadAllText(FilePath(key));
    public bool   Has(string key)    => File.Exists(FilePath(key));
    public void   Delete(string key) { if (Has(key)) File.Delete(FilePath(key)); }

    public void DeleteAll()
    {
        foreach (string f in Directory.GetFiles(_dir, "*.json"))
            File.Delete(f);
    }

    private string FilePath(string key) => Path.Combine(_dir, key + ".json");
}

// ── Factory: выбирает реализацию по платформе ────────────────────────────────

public static class SaveStorageFactory
{
    public static ISaveStorage Create()
    {
        return Application.platform switch
        {
            RuntimePlatform.WebGLPlayer                                       => new PlayerPrefsStorage(),
            RuntimePlatform.WindowsPlayer
                or RuntimePlatform.OSXPlayer
                or RuntimePlatform.LinuxPlayer
                or RuntimePlatform.Android
                or RuntimePlatform.IPhonePlayer                               => new FileStorage(),
            RuntimePlatform.WindowsEditor
                or RuntimePlatform.OSXEditor
                or RuntimePlatform.LinuxEditor                                => new FileStorage("saves_editor"),
            _                                                                 => new PlayerPrefsStorage()
        };
    }
}
