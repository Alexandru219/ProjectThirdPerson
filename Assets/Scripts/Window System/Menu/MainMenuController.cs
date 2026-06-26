using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuController : MenuSystem
{
    public static  MainMenuController Instance;
    
    [Header("Popup")] [SerializeField] protected GameObject confirmNewGamePopup;
    
    

    private void Awake()
    {
        Instance = this;
    }

    private new void Start()
    {
        base.Start();
    }

    private void OnEnable()
    {
     
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
}
