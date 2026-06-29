using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuController : MenuSystem, ISaveable
{
    public static  MainMenuController Instance;
    
    [Header("Popup")] [SerializeField] protected GameObject confirmNewGamePopup;

    [Header("Buttons")] 
    [SerializeField] private Button continueGameBtn;
    [SerializeField] private Button newGameBtn;

    [SerializeField] private bool _isFirstStarted;

    private void Awake()
    {
        Instance = this;
       // _isFirstStarted = PlayerPrefs.GetInt(FirstStartKey, 0) == 1;
    }

    private new IEnumerator Start()
    {
        SaveManager.Instance.LoadGame();
        base.Start();
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(newGameBtn.gameObject);

        ToggleContinueBtn();
    }

    private void OnEnable()
    {
        InputReaderNew.Instance.EnableUI();
        EventSystem.current.SetSelectedGameObject(newGameBtn.gameObject);
    }

    private void ToggleContinueBtn()
    {
        if (_isFirstStarted)
        {
            continueGameBtn.interactable = true;
            continueGameBtn.image.color = new Color(1f, 1f, 1f, 1f);

        }
        else
        {
            continueGameBtn.interactable = false;
            continueGameBtn.image.color = new Color(1f, 1f, 1f, 0.5f);

        }
    }

    public void LoadScene(string targetSceneName)
    {
        if(!_isFirstStarted)
        {
            _isFirstStarted = true;
            SaveManager.Instance.SaveGame();
        }
        SceneLoader.Instance.LoadScene(targetSceneName);
    }

    public void HandleBackNavigation()
    {
        switch (currentState)
        {
            case MenuState.Controls:
                ToggleControlsMenu(false); 
                break;
            case MenuState.Options:
                ToggleSettingsMenu(false); 
                break;
            case MenuState.ConfirmNewGamePopup:
                ToggleConfirmPopup(false);
                break;
            
            case MenuState.Closed:
                
                break;
        }
    }
    private const string FirstStartKey = "IS_FIRST_STARTED";

    public void OnClickNewGame()
    {
        if (_isFirstStarted)
        {
            ToggleConfirmPopup(true);
        }
        else
        {
            _isFirstStarted = true;
           // PlayerPrefs.SetInt(FirstStartKey, true ? 1 : 0);
            //PlayerPrefs.Save();
            SaveManager.Instance.SaveGame();
            SceneLoader.Instance.LoadScene("Scene Test 1");
        }
    }

    public void EnableContinueBtn()
    {
        _isFirstStarted = true;
        SaveManager.Instance.SaveGame();
    }
    
    public void ToggleConfirmPopup(bool state)
    {
        confirmNewGamePopup.SetActive(state);
        baseMenu.SetActive(!state); 

        EventSystem.current.SetSelectedGameObject(null);

        if (state)
        {
            currentState = MenuState.ConfirmNewGamePopup;
            //
        }
        else
        {
            currentState = MenuState.BaseMenu;
            //
        }
        
        
    }
    private bool _isInitializing;
    public ComponentData SaveData()
    {
        var data = new Data
        {
            _isFirstStarted    = _isFirstStarted,
        };
        return new ComponentData(JsonUtility.ToJson(data), typeof(Settings), enabled);
    }

    public void LoadData(ComponentData componentData)
    {
        if (string.IsNullOrEmpty(componentData.componentString)) return;

        _isInitializing = true; 

        var data = JsonUtility.FromJson<Data>(componentData.componentString);

        _isFirstStarted = data._isFirstStarted;
       
    }


    [System.Serializable]
    private struct Data
    {
        public bool _isFirstStarted;
    }

    public bool TypeIsEqual(string type) => type == typeof(Settings).ToString();
}
