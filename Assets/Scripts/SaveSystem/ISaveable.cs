
public interface ISaveable
{
    void LoadData(ComponentData componentData);
    ComponentData SaveData();
    bool TypeIsEqual(string type);
}
