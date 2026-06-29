using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class Settings : MonoBehaviour, ISaveable
{
    [Header("Options Data")]
    [SerializeField] private float soundValue;

    [Header("UI Elements")]
    [SerializeField] private Slider soundSlider;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private TMP_Dropdown languageDropdown;

    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;

    private readonly List<string> _languages   = new() { "Română", "English", "Русский" };
    private readonly List<string> _localeCodes = new() { "ro","en", "ru"};

    private bool _isInitializing;

    private IEnumerator Start()
    {
        _isInitializing = true; 

        InitDropdowns();

        yield return LocalizationSettings.InitializationOperation;

        Locale currentLocale = LocalizationSettings.SelectedLocale;
        if (currentLocale != null)
        {
            int systemLanguageIndex = _localeCodes.IndexOf(currentLocale.Identifier.Code);

            if (systemLanguageIndex != -1)
            {
                languageDropdown.SetValueWithoutNotify(systemLanguageIndex);
            }
        }

        _isInitializing = false;
    }

    /*private void Start()
    {
        InitDropdowns();
    }*/

    private void OnEnable()
    {
        soundSlider.onValueChanged.AddListener(OnSoundChanged);
        qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
    }

    private void OnDisable()
    {
        soundSlider.onValueChanged.RemoveListener(OnSoundChanged);
        qualityDropdown.onValueChanged.RemoveListener(OnQualityChanged);
        languageDropdown.onValueChanged.RemoveListener(OnLanguageChanged);
    }


    private void InitDropdowns()
    {
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new List<string>(QualitySettings.names));

        languageDropdown.ClearOptions();
        languageDropdown.AddOptions(_languages);
    }


    public void OnSoundChanged(float value)
    {
        soundValue = value;

        if (audioMixer != null)
        {
           
            float dB = soundValue > 0.0001f ? Mathf.Log10(soundValue) * 20f : -80f;
            audioMixer.SetFloat("SFX", dB);
        }
        else
        {
            AudioListener.volume = soundValue;
        }

        if (!_isInitializing) SaveManager.Instance.SaveGame();
    }

    public void OnQualityChanged(int index)
    {
        QualitySettings.SetQualityLevel(index, applyExpensiveChanges: true);
        if (!_isInitializing) SaveManager.Instance.SaveGame();
    }

    public void OnLanguageChanged(int index)
    {
        if (index < 0 || index >= _localeCodes.Count) return;
        StartCoroutine(ApplyLocaleCoroutine(_localeCodes[index]));
        if (!_isInitializing) SaveManager.Instance.SaveGame();
    }

    
    private IEnumerator ApplyLocaleCoroutine(string localeCode)
    {
        yield return LocalizationSettings.InitializationOperation;

        Locale locale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);

        if (locale != null)
        {
            LocalizationSettings.SelectedLocale = locale;
            //Debug.Log($"[Settings] Language: {localeCode}");
        }
        
    }


    public ComponentData SaveData()
    {
        var data = new Data
        {
            SoundValue    = soundValue,
            QualityIndex  = qualityDropdown.value,
            LanguageIndex = languageDropdown.value,
        };
        return new ComponentData(JsonUtility.ToJson(data), typeof(Settings), enabled);
    }

    public void LoadData(ComponentData componentData)
    {
        if (string.IsNullOrEmpty(componentData.componentString)) return;

        _isInitializing = true; 

        var data = JsonUtility.FromJson<Data>(componentData.componentString);

        soundValue = data.SoundValue;
        soundSlider.SetValueWithoutNotify(soundValue);
        OnSoundChanged(soundValue);

        qualityDropdown.SetValueWithoutNotify(data.QualityIndex);
        OnQualityChanged(data.QualityIndex);

        languageDropdown.SetValueWithoutNotify(data.LanguageIndex);
        OnLanguageChanged(data.LanguageIndex);

        _isInitializing = false;
    }


    [System.Serializable]
    private struct Data
    {
        public float SoundValue;
        public int   QualityIndex;
        public int   LanguageIndex;
    }

    public bool TypeIsEqual(string type) => type == typeof(Settings).ToString();
}