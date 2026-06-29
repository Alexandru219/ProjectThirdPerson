using UnityEngine;

public class InputController : MonoBehaviour
{
    private PlayerController _controller;
    private InputReaderNew _inputReader;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
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


    private void SubscribeToInput()
    {
        _inputReader.OnMoveEvent += HandleMove;
        _inputReader.OnLookEvent += HandleLook;
        _inputReader.OnJumpStarted += HandleJumpStarted;
        _inputReader.OnSprintStarted += HandleSprintStarted;
        _inputReader.OnSprintCanceled += HandleSprintCanceled;

        _inputReader.OnOpenUI += HandleUIOpened;
        _inputReader.OnCloseUI += HandleUIClosed;

        _inputReader.OnCancelMenu += HandleBackNavigation;
        _inputReader.OnNextSchemeMenu += HandleNextScheme;
        _inputReader.OnPreviousSchemeMenu += HandlePreviousScheme;

        _inputReader.OnNavigateEvent += HandleNavigate;
        _inputReader.OnInteractEvent += HandleInteract;

    }

    

    private void UnsubscribeFromInput()
    {
        if (_inputReader == null)
            return;

        _inputReader.OnMoveEvent -= HandleMove;
        _inputReader.OnLookEvent -= HandleLook;
        _inputReader.OnJumpStarted -= HandleJumpStarted;
        _inputReader.OnSprintStarted -= HandleSprintStarted;
        _inputReader.OnSprintCanceled -= HandleSprintCanceled;

        _inputReader.OnOpenUI -= HandleUIOpened;
        _inputReader.OnCloseUI -= HandleUIClosed;

        _inputReader.OnCancelMenu -= HandleBackNavigation;
        _inputReader.OnNextSchemeMenu -= HandleNextScheme;
        _inputReader.OnPreviousSchemeMenu -= HandlePreviousScheme;

        _inputReader.OnNavigateEvent -= HandleNavigate;
        _inputReader.OnInteractEvent -= HandleInteract;
        
        
    }

    private void StartAttack()
    {
        if (!_inputReader.IsGameplayActive)  return;
            
           // _controller.StartAttack();
    }
    
    private void HandleMove(Vector2 input)
    {
       // if (!_inputReader.IsGameplayActive)
           // return;

       // _controller.SetMoveInput(input);
    }

    private void HandleLook(Vector2 input)
    {
       // if (!_inputReader.IsGameplayActive)
            return;

       // _controller.ExecuteLook(input);
    }

    private void HandleJumpStarted()
    {
        if (!_inputReader.IsGameplayActive)
            return;

       // _controller.Jump();
    }

    private void HandleSprintStarted()
    {
        if (!_inputReader.IsGameplayActive)
            return;

      //  _controller.SetSprint(true);
    }

    private void HandleSprintCanceled()
    {
       // _controller.SetSprint(false);
    }

    private void HandleInteract()
    {
        if (!_inputReader.IsGameplayActive)
            return;

        if (InteractionModule.Instance != null)
            InteractionModule.Instance.StartInteraction();
    }


    private void HandleUIOpened()
    {
       
        
        if (PauseMenuController.Instance != null)
            PauseMenuController.Instance.OpenPauseMenu();
    }

    private void HandleUIClosed()
    {
     
        
        if (PauseMenuController.Instance != null)
            PauseMenuController.Instance.ClosePauseMenu();
    }

    private void HandleBackNavigation()
    {
        if (PauseMenuController.Instance != null)
            PauseMenuController.Instance.HandleBackNavigation();

        if (MainMenuController.Instance != null)
            MainMenuController.Instance.HandleBackNavigation();
    }

    private void HandleNextScheme()
    {
        if (PauseMenuController.Instance != null)
            PauseMenuController.Instance.NextSchemeControls();
    }

    private void HandlePreviousScheme()
    {
        if (PauseMenuController.Instance != null)
            PauseMenuController.Instance.PreviousSchemeControls();
    }

    private void HandleNavigate(Vector2 value)
    {
        if (WindowManager.Instance != null)
            WindowManager.Instance.SelectFirst();
    }
}