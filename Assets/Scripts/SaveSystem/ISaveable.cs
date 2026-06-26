/// <summary>
/// Контракт для любого компонента, который хочет участвовать в сохранении.
/// Реализуй на своих MonoBehaviour-компонентах (Health, Inventory, и т.д.).
/// </summary>
public interface ISaveable
{
    void          LoadData(ComponentData componentData);
    ComponentData SaveData();
    bool          TypeIsEqual(string type);
}
