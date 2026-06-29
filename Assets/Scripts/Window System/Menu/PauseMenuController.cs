using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class PauseMenuController : MenuSystem
{
    public static PauseMenuController Instance;
    //[SerializeField] private InputReaderNew inputReader;
    public bool isPause;
    [Header("UI Menus")]
    [SerializeField] private GameObject pauseMenu;
   // [SerializeField] private GameObject controlsMenu; 
   

    
    [Header("Popup")] [SerializeField] private GameObject savePopup;
    
    [Header("First Selected Elements")]
    [SerializeField] private GameObject firstPauseButton;
    [SerializeField] private GameObject firstControlsButton;
    [SerializeField] private GameObject firstSettingsButton;

    [Space]
    public UnityEvent onMenuClose;
    public UnityEvent onMenuOpen;
    

    private void Awake()
    {
        Instance = this;
        
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (controlsMenu != null) controlsMenu.SetActive(false);
        if (optionsMenu != null) optionsMenu.SetActive(false);
        if (savePopup != null) savePopup.SetActive(false);
    }

    private new void Start()
    {
        base.Start();
    }

    public void LoadScene(string sceneName)
    {
        SceneLoader.Instance.LoadScene(sceneName);
    }
    

    private void OnEnable()
    {
        onMenuOpen.AddListener(OpenPauseMenu);
        onMenuClose.AddListener(ClosePauseMenu);
    }

    private void OnDisable()
    {
        onMenuOpen.RemoveListener(OpenPauseMenu);
        onMenuClose.RemoveListener(ClosePauseMenu);
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
            case MenuState.BaseMenu:
                ContinueGame(); 
                break;
            case  MenuState.SavePopup:
                ToggleSavePopup(false);
                break;
            
            case MenuState.Closed:
                
                break;
        }
    }



    public void OpenPauseMenu()
    {
        if (TutorialManager.Instance != null)
        {
            if(TutorialManager.Instance.isPopupActive) return;
        }
        
        InputReaderNew.Instance.EnableUI();
        isPause = true;
        if(controlsMenu != null) controlsMenu.SetActive(false);
        if(controlsMenu != null) optionsMenu.SetActive(false);
        if(savePopup.activeSelf) savePopup.SetActive(false);
        pauseMenu.SetActive(true);

        currentState = MenuState.BaseMenu;
        WindowManager.Instance.OpenWindow(baseWindow);
        SetFocusTo(firstPauseButton);
    }

    public void ClosePauseMenu()
    {
        if (TutorialManager.Instance != null)
        {
            if(TutorialManager.Instance.isPopupActive) return;
        }
        
        isPause = false;
        pauseMenu.SetActive(false);
        controlsMenu.SetActive(false);
        optionsMenu.SetActive(false);
        if(savePopup.activeSelf) savePopup.SetActive(false);
        currentState = MenuState.Closed;
    }

    public void ContinueGame()
    {
        inputReader.EnableGameplay();
        ClosePauseMenu();
    }


 

   
    
    public void ToggleSavePopup(bool state)
    {
        savePopup.SetActive(state);
        pauseMenu.SetActive(!state); 

        EventSystem.current.SetSelectedGameObject(null);

        if (state)
        {
            currentState = MenuState.SavePopup;
            //SetFocusTo(firstSettingsButton);
        }
        else
        {
            currentState = MenuState.BaseMenu;
            //SetFocusTo(firstPauseButton);
        }
    }

    public void SaveGamePopup()
    {
        SaveManager.Instance.SaveGame();
        ToggleSavePopup(false);
        ClosePauseMenu();
        inputReader.EnableGameplay();
    }
   

    public void NextSchemeControls()
    {
        currentIndex = (currentIndex + 1) % schemeMenu.Count;
        UpdateSchemeControls();
    }

    public void PreviousSchemeControls()
    {
        currentIndex = (currentIndex - 1 + schemeMenu.Count) % schemeMenu.Count;
        UpdateSchemeControls();
    }
    
    private void UpdateSchemeControls()
    {
        for (int i = 0; i < schemeMenu.Count; i++)
        {
            schemeMenu[i].SetActive(i == currentIndex);
        }
    }

    
    private void SetFocusTo(GameObject targetButton)
    {
        if (targetButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(targetButton);
        }
    }
}