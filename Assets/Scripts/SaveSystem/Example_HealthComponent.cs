using UnityEngine;

/// <summary>
/// Пример компонента, который реализует ISaveable.
/// Он НЕ знает ни о файлах, ни о PlayerPrefs — только сериализует свои данные.
/// </summary>
public class ExempleComponent : MonoBehaviour, ISaveable
{
    [SerializeField] private float maxHealth = 100f;
    private float _currentHealth;

    private void Awake() => _currentHealth = maxHealth;

    // ── ISaveable ─────────────────────────────────────────────────────────────

    public ComponentData SaveData()
    {
        var data = new HealthData { current = _currentHealth, max = maxHealth };
        return new ComponentData(JsonUtility.ToJson(data), typeof(ExempleComponent), enabled);
    }

    public void LoadData(ComponentData componentData)
    {
        if (string.IsNullOrEmpty(componentData.componentString)) return;

        var data = JsonUtility.FromJson<HealthData>(componentData.componentString);
        _currentHealth = data.current;
        maxHealth      = data.max;
        enabled        = componentData.isEnable;
    }

    public bool TypeIsEqual(string type) => type == typeof(ExempleComponent).ToString();


    public void TakeDamage(float amount)
    {
        _currentHealth = Mathf.Max(0f, _currentHealth - amount);
    }


    [System.Serializable]
    private struct HealthData
    {
        public float current;
        public float max;
    }
}
