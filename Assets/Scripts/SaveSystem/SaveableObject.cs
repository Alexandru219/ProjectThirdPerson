using System;
using System.Collections.Generic;
using UnityEngine;


public class SaveableObject : MonoBehaviour
{
    [SerializeField] private string objectId;
    [SerializeField] private bool   savePos;

    private ISaveStorage _storage;

    private readonly SaveableObjectData _objectData   = new SaveableObjectData();
    private readonly List<ISaveable>    _allSaveable  = new List<ISaveable>();


    public void Init(ISaveStorage storage)
    {
        _storage = storage;
    }

    public void SaveData()
    {
        EnsureStorageReady();

        var saveables = GetComponentsInChildren<ISaveable>(); // array local, nu câmp
        _objectData.saveableComponents.Clear();
        _objectData.objectID = objectId;
        _objectData.isActive = gameObject.activeSelf;

        if (savePos)
            _objectData.transformData = new TransformData(transform);

        foreach (var saveable in saveables)
            _objectData.saveableComponents.Add(saveable.SaveData());

        _storage.Write(objectId, JsonUtility.ToJson(_objectData));
    }

    public void LoadData()
    {
        EnsureStorageReady();

        if (!_storage.Has(objectId)) return;

        var loaded   = JsonUtility.FromJson<SaveableObjectData>(_storage.Read(objectId));
        var saveables = GetComponentsInChildren<ISaveable>(); // array local, nu câmp

        if (savePos)
        {
            transform.localPosition = loaded.transformData.position;
            transform.localRotation = loaded.transformData.rotation;
        }

        foreach (var saveable in saveables)
        {
            var componentData = loaded.saveableComponents
                .Find(sc => saveable.TypeIsEqual(sc.componentTypeString));
            saveable.LoadData(componentData);
        }

        gameObject.SetActive(loaded.isActive);
    }

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(objectId))
            objectId = Guid.NewGuid().ToString();

        _objectData.objectID = objectId;
    }

    private void EnsureStorageReady()
    {
        if (_storage == null)
            throw new InvalidOperationException(
                $"[SaveableObject] '{gameObject.name}' has no storage assigned.");
    }

    private void CollectSaveables()
    {
        _allSaveable.Clear();                                          
        _allSaveable.AddRange(GetComponentsInChildren<ISaveable>());
        _objectData.saveableComponents.Clear();
    }

   
}
