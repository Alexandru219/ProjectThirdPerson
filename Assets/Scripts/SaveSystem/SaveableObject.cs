using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Прикрепляй на любой GameObject, который нужно сохранять.
///
/// ВАЖНО: этот класс НЕ знает, куда именно сохраняются данные.
/// Он получает ISaveStorage снаружи (через Init) — от SaveManager.
/// </summary>
public class SaveableObject : MonoBehaviour
{
    [SerializeField] private string objectId;
    [SerializeField] private bool   savePos;

    // Хранилище инжектируется SaveManager-ом, не создаётся здесь
    private ISaveStorage _storage;

    private readonly SaveableObjectData _objectData   = new SaveableObjectData();
    private readonly List<ISaveable>    _allSaveable  = new List<ISaveable>();

    // ── Public API (вызывается только SaveManager-ом) ─────────────────────────

    /// <summary>
    /// Устанавливает хранилище перед Save / Load.
    /// SaveManager вызывает это один раз при старте.
    /// </summary>
    public void Init(ISaveStorage storage)
    {
        _storage = storage;
    }

    public void SaveData()
    {
        EnsureStorageReady();
        CollectSaveables();

        _objectData.objectID  = objectId;
        _objectData.isActive  = gameObject.activeSelf;

        if (savePos)
            _objectData.transformData = new TransformData(transform);

        foreach (var saveable in _allSaveable)
            _objectData.saveableComponents.Add(saveable.SaveData());

        // ✅ Gameplay-код передаёт данные в хранилище через интерфейс — не пишет напрямую
        _storage.Write(objectId, JsonUtility.ToJson(_objectData));
    }

    public void LoadData()
    {
        EnsureStorageReady();

        // ✅ Читает через интерфейс — не знает, файл это или PlayerPrefs
        if (!_storage.Has(objectId)) return;

        var loaded = JsonUtility.FromJson<SaveableObjectData>(_storage.Read(objectId));

        CollectSaveables();

        if (savePos)
        {
            transform.localPosition = loaded.transformData.position;
            transform.localRotation = loaded.transformData.rotation;
        }

        foreach (var saveable in _allSaveable)
        {
            var componentData = loaded.saveableComponents
                .Find(sc => saveable.TypeIsEqual(sc.componentTypeString));
            saveable.LoadData(componentData);
        }

        gameObject.SetActive(loaded.isActive);
    }

    // ── Editor helper ─────────────────────────────────────────────────────────

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(objectId))
            objectId = Guid.NewGuid().ToString();

        _objectData.objectID = objectId;
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    /// <summary>
    /// Пересобирает список ISaveable каждый раз.
    /// ✅ Исправлен баг оригинала: список теперь сначала очищается, потом заполняется.
    /// </summary>
    private void CollectSaveables()
    {
        _allSaveable.Clear();                                           // ← фикс бага
        _allSaveable.AddRange(GetComponentsInChildren<ISaveable>());
        _objectData.saveableComponents.Clear();
    }

    private void EnsureStorageReady()
    {
        if (_storage == null)
            throw new InvalidOperationException(
                $"[SaveableObject] '{gameObject.name}': storage не задан. " +
                "Вызови Init(storage) перед Save/Load.");
    }
}
