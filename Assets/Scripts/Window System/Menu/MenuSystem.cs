using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class MenuSystem : MonoBehaviour
{
    [FormerlySerializedAs("pauseMenu")] [SerializeField] protected GameObject baseMenu;
    [SerializeField] protected GameObject controlsMenu; 
    [SerializeField] protected GameObject optionsMenu;
    
    
    
    [SerializeField] protected InputReaderNew inputReader;
    public BaseWindow baseWindow;
    
    [SerializeField] protected List<GameObject> schemeMenu;
    protected int currentIndex = 0;
    
    [Header("Menu State")]
    [SerializeField] protected MenuState currentState = MenuState.Closed;
    protected enum MenuState { Closed, BaseMenu, Controls, Options, SavePopup, ConfirmNewGamePopup }
    
    protected void Start()
    {
        if(inputReader == null) 
        {
            inputReader = FindAnyObjectByType<InputReaderNew>();
        }
        
    }
    
    

    public void ToggleControlsMenu(bool state)
    {
        if(controlsMenu != null) controlsMenu.SetActive(state);
        baseMenu.SetActive(!state); 
        EventSystem.current.SetSelectedGameObject(null);

        if (state)
        {
            currentState = MenuState.Controls;
            //SetFocusTo(firstControlsButton);
        }
        else
        {
            currentState = MenuState.BaseMenu;
            //SetFocusTo(firstPauseButton);
        }
    }
    

    
    public void ToggleSettingsMenu(bool state)
    {
        optionsMenu.SetActive(state);
        baseMenu.SetActive(!state); 

        EventSystem.current.SetSelectedGameObject(null);

        if (state)
        {
            currentState = MenuState.Options;
          //  SetFocusTo(firstSettingsButton);
        }
        else
        {
            currentState = MenuState.BaseMenu;
            //SetFocusTo(firstPauseButton);
        }
    }
    
  

    public void EnableMenu(GameObject menu)
    {
        menu.SetActive(true);
    }


}
