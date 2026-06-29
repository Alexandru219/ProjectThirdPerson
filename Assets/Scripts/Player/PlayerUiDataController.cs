using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUiDataController : MonoBehaviour, ISaveable
{
    public static PlayerUiDataController Instance;
    
    [Header("Player Values")] [SerializeField]
    private float balanceValue;
    [SerializeField] private float staminaValue;
    [SerializeField] private float maxStaminaValue = 10f;
    
    [Header("UI Data")] 
    [SerializeField] private Slider sliderStamina;
    [SerializeField] private TextMeshProUGUI textBalance;

    public float BalanceValue
    {
        get { return balanceValue; }
        set { balanceValue = value; }
    }

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
       // textBalance.text = balanceValue.ToString();
        yield return new WaitForEndOfFrame();
        sliderStamina.value = staminaValue;
        textBalance.text = balanceValue.ToString();
    }
    
    

    public void UpdateStamina(float value)
    {
        if (staminaValue != 0)
        {
            staminaValue -= value;
            sliderStamina.value = staminaValue;
        }
    }

    
    public void ResetStamina()
    {
        staminaValue = maxStaminaValue;
        sliderStamina.value = staminaValue;
    }

    public void IncreaseBalance(float value)
    {
        balanceValue += value;
        textBalance.text = balanceValue.ToString();
    }

    public void DecreaseBalance(float value)
    {
        balanceValue -= value;
        textBalance.text = balanceValue.ToString();
    }
    
    public ComponentData SaveData()
    {
        var data = new Data { BalanceValue = balanceValue, StaminaValue = staminaValue };
        return new ComponentData(JsonUtility.ToJson(data), typeof(TestInteraction), enabled);
    }

    public void LoadData(ComponentData componentData)
    {
        if (string.IsNullOrEmpty(componentData.componentString)) return;
        var data = JsonUtility.FromJson<Data>(componentData.componentString);
        balanceValue = data.BalanceValue;
        staminaValue = data.StaminaValue;
    }

    public bool TypeIsEqual(string type) => type == typeof(TestInteraction).ToString();

    [System.Serializable]
    private struct Data
    {
        public float BalanceValue;
        public float StaminaValue;
    }
}
