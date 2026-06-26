using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    //private PlayerInput playerInput;
    public bool IsGamepad { get; private set; }

    public GameObject LastHoveredElement { get; set; } 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        //playerInput = GetComponent<PlayerInput>();
    }
    

    private void HandleDeviceSwitchUIFocus()
    {
        if (IsGamepad)
        {
            GameObject targetSelect = LastHoveredElement != null 
                ? LastHoveredElement 
                : EventSystem.current.currentSelectedGameObject;

            EventSystem.current.SetSelectedGameObject(targetSelect);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}