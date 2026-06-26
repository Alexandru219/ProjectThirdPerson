using System.Collections;
using UnityEngine;

public class TestInteraction : Interactable, ISaveable
{
    [Header("Save Example")]
    [SerializeField] private bool materialIsUpdate;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material updateMaterial;
    
    private const string INTERACT_ACHIEVEMENT_ID = "ACH_FIRST_INTERACTION";
    
    private new IEnumerator Start()
    {
        base.Start();
        onInteract.AddListener(TestInteract);
        yield return new WaitForSeconds(0.2f);
        if(materialIsUpdate)
        {
            meshRenderer.material = updateMaterial;
        }
        else
        {
            meshRenderer.material = defaultMaterial;
        }
    }

    private void TestInteract()
    {
        if (!materialIsUpdate)
        {
            ToggleMaterial(true);
            
        }
        else
        {
            ToggleMaterial(false);
        }
        
       // SaveManager.Instance.SaveGame();
    }

    private void ToggleMaterial(bool value)
    {
        if (value)
        {
            materialIsUpdate = true;
            meshRenderer.material = updateMaterial;
        }
        else
        {
            materialIsUpdate = false;
            meshRenderer.material = defaultMaterial;
        }
    }
    
    public ComponentData SaveData()
    {
        var data = new Data { materialIsUpdate = materialIsUpdate};
        return new ComponentData(JsonUtility.ToJson(data), typeof(TestInteraction), enabled);
    }

    public void LoadData(ComponentData componentData)
    {
        if (string.IsNullOrEmpty(componentData.componentString)) return;
        var data = JsonUtility.FromJson<Data>(componentData.componentString);
        materialIsUpdate = data.materialIsUpdate;
    }

    public bool TypeIsEqual(string type) => type == typeof(TestInteraction).ToString();

    [System.Serializable]
    private struct Data
    {
        public bool materialIsUpdate;
    }
}