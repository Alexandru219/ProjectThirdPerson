using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrackerDeviceManager : MonoBehaviour
{
    public static TrackerDeviceManager Instance { get; private set; }

    public enum DeviceType
    {
        KeyboardMouse,
        Gamepad
    }
    public enum DeviceNavigateUI 
    { 
        Keyboard, 
        Mouse 
    }

    public enum GamepadType
    {
        None, 
        Xbox, 
        PlayStation
    }

    [field: SerializeField] public DeviceType CurrentDevice { get; private set; } = DeviceType.KeyboardMouse;
    [field: SerializeField] public DeviceNavigateUI CurrentNavigateUIDevice { get; private set; } = DeviceNavigateUI.Mouse;
    [field: SerializeField] public GamepadType  CurrentGamepadType { get; private set; } = GamepadType.None;

    [Header("Gamepad noise filter")]
    [SerializeField] [Range(0.05f, 0.4f)] private float _gamepadDeadzone = 0.2f;

    public bool IsGamepad => CurrentDevice == DeviceType.Gamepad;
    public bool IsMouse => CurrentNavigateUIDevice == DeviceNavigateUI.Mouse;

    public event Action<DeviceType> OnDeviceChanged;
    public event Action<DeviceNavigateUI> OnDeviceNavigateUIChanged;


    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()  => InputSystem.onActionChange += HandleActionChange;
    private void OnDisable() => InputSystem.onActionChange -= HandleActionChange;

  

    private void HandleActionChange(object obj, InputActionChange change)
    {
        if (change != InputActionChange.ActionPerformed) return;

        InputAction  action  = (InputAction)obj;
        InputControl activeControl = action.activeControl;
        if (activeControl == null) return;

        InputDevice device = activeControl.device;
        DetectAndSetDevice(device);
        DetectNavigateUIDevice(device);
    }

    private void DetectAndSetDevice(InputDevice device)
    {
        DeviceType detectedDevice = CurrentDevice;
        GamepadType detectedGamepad = CurrentGamepadType;

        if (device is Gamepad gamepad)
        {
            if (!IsGamepadInputReal(gamepad)) return;

            detectedDevice = DeviceType.Gamepad;

            string deviceName = device.name.ToLower();
            string deviceProduct = device.description.product != null
                ? device.description.product.ToLower()
                : string.Empty;

            bool isPlayStation = deviceName.Contains("dual") || deviceName.Contains("sony") ||
                                 deviceName.Contains("playstation") || deviceProduct.Contains("dual") ||
                                 deviceProduct.Contains("sony");

            detectedGamepad = isPlayStation ? GamepadType.PlayStation : GamepadType.Xbox;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (device is Keyboard || device is Mouse)
        {
            detectedDevice = DeviceType.KeyboardMouse;
            detectedGamepad = GamepadType.None;

            if (InputReaderNew.Instance.IsUIActive)
            {
                if (!IsGamepad)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }

        }

        if (detectedDevice != CurrentDevice || detectedGamepad != CurrentGamepadType)
        {
            CurrentDevice = detectedDevice;
            CurrentGamepadType = detectedGamepad;

            Debug.Log($"[TrackerDeviceManager] Device: {CurrentDevice} | Sub-Type: {CurrentGamepadType}");
            OnDeviceChanged?.Invoke(CurrentDevice);
        }
    }
    
    private void DetectNavigateUIDevice(InputDevice device)
    {
        if (!InputReaderNew.Instance.IsUIActive) return;

        DeviceNavigateUI detectedDevice = CurrentNavigateUIDevice;

        if (device is Mouse)
        {
            detectedDevice = DeviceNavigateUI.Mouse;
           // Cursor.visible = true;
           
        }
        else if (device is Keyboard)
        {
            detectedDevice = DeviceNavigateUI.Keyboard;
           // Cursor.visible = false;
        }

        if (detectedDevice != CurrentNavigateUIDevice)
        {
            CurrentNavigateUIDevice = detectedDevice;
            Debug.Log($"[TrackerDeviceManager] Navigate UI device: {CurrentNavigateUIDevice}");
            OnDeviceNavigateUIChanged?.Invoke(CurrentNavigateUIDevice);
        }
    }

    

    private bool IsGamepadInputReal(Gamepad gamepad)
    {
     
        if (gamepad.leftStick.ReadValue().magnitude > _gamepadDeadzone) return true;
        if (gamepad.rightStick.ReadValue().magnitude > _gamepadDeadzone) return true;
        if (gamepad.leftTrigger.ReadValue() > _gamepadDeadzone) return true;
        if (gamepad.rightTrigger.ReadValue() > _gamepadDeadzone) return true;

        if (gamepad.buttonSouth.isPressed || gamepad.buttonNorth.isPressed ||
            gamepad.buttonEast.isPressed || gamepad.buttonWest.isPressed ||
            gamepad.startButton.isPressed || gamepad.selectButton.isPressed ||
            gamepad.leftShoulder.isPressed  || gamepad.rightShoulder.isPressed||
            gamepad.leftStickButton.isPressed || gamepad.rightStickButton.isPressed) return true;

        // D-Pad
        if (gamepad.dpad.up.isPressed || gamepad.dpad.down.isPressed ||
            gamepad.dpad.left.isPressed || gamepad.dpad.right.isPressed) return true;

        return false;
    }
}
