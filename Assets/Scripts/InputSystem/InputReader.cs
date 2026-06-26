using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
public enum StartInputActionMap
{
    GameplayMap,
    UIMap
}
public class InputReaderNew : MonoBehaviour, InputActions.IGameplayActions, InputActions.IUIActions
{
    public static InputReaderNew Instance { get; private set; }
    public StartInputActionMap startInputActionMap;
    // Input Asset
    private InputActions _inputActions;

    //  Gameplay Events 
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;
    public event Action OnJumpStarted;
    public event Action OnJumpCanceled;
    public event Action OnSprintStarted;
    public event Action OnSprintCanceled;
    public event Action OnFireStarted;
    public event Action OnFireCanceled;
    public event Action OnOpenUI;
    public event Action OnInteractEvent;

    // UI Events 
    public event Action OnCloseUI;         
    public event Action<Vector2> OnNavigateEvent;
    public event Action OnSubmit;
    public event Action OnCancel;
    public event Action OnCancelMenu;
    public event Action OnNextSchemeMenu;
    public event Action OnPreviousSchemeMenu;
    public Vector2 LookValue { get; private set; }

   
    // State
    public bool IsGameplayActive { get; private set; }
    public bool IsUIActive { get; private set; }

    //  Unity Lifecycle 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
      //  DontDestroyOnLoad(gameObject);

        _inputActions = new InputActions();

        _inputActions.Gameplay.SetCallbacks(this);
        _inputActions.UI.SetCallbacks(this);

        var uiInputModule = FindAnyObjectByType<InputSystemUIInputModule>();
        if (uiInputModule != null)
        {
            uiInputModule.actionsAsset = _inputActions.asset;
        }
    }

    private void OnEnable()
    {
        if (startInputActionMap == StartInputActionMap.GameplayMap)
        {
            EnableGameplay();
        }
        else
        {
            EnableUI();
        }
    }

    private void OnDisable()
    {
        DisableAll();
    }

    private void OnDestroy()
    {
        _inputActions?.Dispose();
    }
    
    public void EnableGameplay()
    {
        _inputActions.UI.Disable();
        _inputActions.Gameplay.Enable();

        IsGameplayActive = true;
        IsUIActive = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("[InputReader] Action Map: GAMEPLAY");
    }

    public void EnableUI()
    {
        _inputActions.Gameplay.Disable();
        _inputActions.UI.Enable();

        IsGameplayActive = false;
        IsUIActive = true;


        if (TrackerDeviceManager.Instance != null)
        {
            if (!TrackerDeviceManager.Instance.IsGamepad)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

    }

    public void DisableAll()
    {
        _inputActions.Gameplay.Disable();
        _inputActions.UI.Disable();

        IsGameplayActive = false;
        IsUIActive = false;

        Debug.Log("[InputReader] All action map are disabled.");
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        OnLookEvent?.Invoke(context.ReadValue<Vector2>());
        LookValue = context.ReadValue<Vector2>();

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
            OnJumpStarted?.Invoke();
        else if (context.canceled)
            OnJumpCanceled?.Invoke();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
            OnSprintStarted?.Invoke();
        else if (context.canceled)
            OnSprintCanceled?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
            OnInteractEvent?.Invoke();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
    }

    public void OnStartPauseMenu(InputAction.CallbackContext context)
    {
       // PauseMenuController.Instance.onMenuOpen.Invoke();
       if (context.started)
       {
           OnOpenUI?.Invoke();
           EnableUI(); 
       }
    }
    

    //  IUIActions Implementation

    public void OnStopPauseMenu(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            EnableGameplay(); 
            OnCloseUI?.Invoke();
        }
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
    }

    void InputActions.IUIActions.OnCancel(InputAction.CallbackContext context)
    {
    }

    void InputActions.IUIActions.OnSubmit(InputAction.CallbackContext context)
    {
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnNavigateEvent?.Invoke(context.ReadValue<Vector2>());
        }
    }

    public void OnPreviousMenu(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnCancelMenu?.Invoke(); 
        }
    }

    public void OnNextSchemeControls(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnNextSchemeMenu?.Invoke(); 
        }
    }

    public void OnPreviousSchemeControls(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnPreviousSchemeMenu?.Invoke(); 
        }
    }

    public void OnSubmitAction(InputAction.CallbackContext context)
    {
        if (context.started)
            OnSubmit?.Invoke();
    }

    public void OnCancelAction(InputAction.CallbackContext context)
    {
        
    }
}
