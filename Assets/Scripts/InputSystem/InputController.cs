using UnityEngine;

//[RequireComponent(typeof(FPSController))]
public class InputController : MonoBehaviour
{
    private FPSController _fpsController;
    private InputReaderNew _inputReader;

    private void Awake()
    {
        _fpsController = GetComponent<FPSController>();
    }

    private void Start()
    {
        _inputReader = InputReaderNew.Instance;

        if (_inputReader == null)
        {
            Debug.LogError("[InputController] InputReaderNew.Instance este null!");
            enabled = false;
            return;
        }

        SubscribeToInput();
    }
    
    

    private void OnDestroy()
    {
        UnsubscribeFromInput();
    }

    // ─── Input Subscriptions ──────────────────────────────────────────────────

    private void SubscribeToInput()
    {
        _inputReader.OnMoveEvent += HandleMove;
        _inputReader.OnLookEvent += HandleLook;
        _inputReader.OnJumpStarted += HandleJumpStarted;
        _inputReader.OnSprintStarted += HandleSprintStarted;
        _inputReader.OnSprintCanceled += HandleSprintCanceled;
        _inputReader.OnOpenUI += HandleUIOpened;
        _inputReader.OnCloseUI += HandleUIClosed;
        _inputReader.OnCancelMenu += HandleUIBackMenu;
        _inputReader.OnNextSchemeMenu += NextControlsMenu;
        _inputReader.OnPreviousSchemeMenu += PreviousControlsMenu;
        _inputReader.OnNavigateEvent += CheckNavigateUI;
        _inputReader.OnInteractEvent += InteractEvent;
    }

    


    private void UnsubscribeFromInput()
    {
        if (_inputReader == null) return;

        _inputReader.OnMoveEvent -= HandleMove;
        _inputReader.OnLookEvent -= HandleLook;
        _inputReader.OnJumpStarted -= HandleJumpStarted;
        _inputReader.OnSprintStarted -= HandleSprintStarted;
        _inputReader.OnSprintCanceled -= HandleSprintCanceled;
        _inputReader.OnOpenUI -= HandleUIOpened;
        _inputReader.OnCloseUI -= HandleUIClosed;
        _inputReader.OnCancelMenu -= HandleUIBackMenu;
        _inputReader.OnNavigateEvent -= CheckNavigateUI;
        _inputReader.OnInteractEvent -= InteractEvent;

    }

    // Event Handlers 

    private void HandleUIBackMenu()
    {
        if(PauseMenuController.Instance != null) PauseMenuController.Instance.HandleBackNavigation();
        
        if(MainMenuController.Instance != null) MainMenuController.Instance.HandleBackNavigation();
    }
    
    private void HandleMove(Vector2 input)
    {
        if (!_inputReader.IsGameplayActive) return;
        _fpsController.SetMovementCommand(input);
    }
    
    private void InteractEvent()
    {
        InteractionModule.Instance.StartInteraction();
    }

    private void HandleLook(Vector2 delta)
    {
        if (!_inputReader.IsGameplayActive) return;
        _fpsController.ExecuteLook(delta);
       // _fpsController.GamepadExecuteLook(delta);
    }

    private void HandleJumpStarted()
    {
        if (!_inputReader.IsGameplayActive) return;
        _fpsController.ExecuteJump();
    }

    private void HandleSprintStarted()
    {
        if (!_inputReader.IsGameplayActive) return;
        _fpsController.SetSprintCommand(true);
    }

    private void HandleSprintCanceled()
    {
        _fpsController.SetSprintCommand(false);
    }
    
    private void PreviousControlsMenu()
    {
       if(PauseMenuController.Instance != null) PauseMenuController.Instance.PreviousSchemeControls();
    }

    private void NextControlsMenu()
    {
        if(PauseMenuController.Instance != null) PauseMenuController.Instance.NextSchemeControls();

    }
    
    private void CheckNavigateUI(Vector2 obj)
    {
        WindowManager.Instance.SelectFirst();
    }

    private void HandleUIOpened()
    {
        _fpsController.ResetMovementState();
        //_fpsController.enabled = false; 

        if (PauseMenuController.Instance != null)
        {
            PauseMenuController.Instance.OpenPauseMenu();
        }
    }

    private void HandleUIClosed()
    {
        _fpsController.enabled = true; 

        if (PauseMenuController.Instance != null)
        {
            PauseMenuController.Instance.ClosePauseMenu();
        }
    }
}