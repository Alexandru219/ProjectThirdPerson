using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private Image _alertSaveGame;

    [Header("Alert duration (sec)")]
    [SerializeField] private float _alertDuration = 1.0f;

    private ISaveStorage  _storage;
    private List<SaveableObject> _cachedObjects = new List<SaveableObject>();

    

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
       // DontDestroyOnLoad(gameObject);

        _storage = SaveStorageFactory.Create();
        RefreshCache(); 
        
        
    }

   


    public void SaveGame()
    {
        foreach (var obj in _cachedObjects)
        {
            obj.Init(_storage);   
            obj.SaveData();
        }

        Debug.Log("[SaveManager] Game saved.");
        StartCoroutine(ShowSaveAlert());
    }

    public void LoadGame()
    {
        foreach (var obj in _cachedObjects)
        {
            obj.Init(_storage);
            obj.LoadData();
        }

        Debug.Log("[SaveManager] Game loaded.");
    }

    public void LoadObject(SaveableObject saveableObject)
    {
        saveableObject.Init(_storage);
        saveableObject.LoadData();
    }

    public void LoadObjects(IEnumerable<SaveableObject> objects)
    {
        foreach (var obj in objects)
        {
            obj.Init(_storage);
            obj.LoadData();
        }
    }

    public void DeleteAllData()
    {
        _storage.DeleteAll();
        Debug.Log("[SaveManager] All save data deleted.");
    }

    
    public void RefreshCache()
    {
        _cachedObjects.Clear();
        _cachedObjects.AddRange(FindObjectsOfType<SaveableObject>(includeInactive: true));
        Debug.Log($"[SaveManager] Cached {_cachedObjects.Count} saveable objects.");
    }


    private IEnumerator ShowSaveAlert()
    {
        if (_alertSaveGame == null) yield break;
        _alertSaveGame.gameObject.SetActive(true);
        yield return new WaitForSeconds(_alertDuration);
        _alertSaveGame.gameObject.SetActive(false);
    }
}
