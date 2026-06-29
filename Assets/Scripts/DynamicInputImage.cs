using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DynamicInputImage : MonoBehaviour
{
    [Header("Device Sprites")]
    [SerializeField] private Sprite keyboardSprite;
    [SerializeField] private Sprite xboxSprite;
    [SerializeField] private Sprite playstationSprite;

    private Image _targetImage;

    private void Awake()
    {
        _targetImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        if (TrackerDeviceManager.Instance != null)
        {
            UpdateImage(TrackerDeviceManager.Instance.CurrentDevice);
            TrackerDeviceManager.Instance.OnDeviceChanged += UpdateImage;
        }
    }

    private void OnDisable()
    {
        if (TrackerDeviceManager.Instance != null)
        {
            TrackerDeviceManager.Instance.OnDeviceChanged -= UpdateImage;
        }
    }

    private void UpdateImage(TrackerDeviceManager.DeviceType deviceType)
    {
        if (deviceType == TrackerDeviceManager.DeviceType.Gamepad)
        {
            if (TrackerDeviceManager.Instance.CurrentGamepadType == TrackerDeviceManager.GamepadType.PlayStation)
            {
                if (playstationSprite != null) _targetImage.sprite = playstationSprite;
            }
            else 
            {
                if (xboxSprite != null) _targetImage.sprite = xboxSprite;
            }
        }
        else // KeyboardMouse
        {
            if (keyboardSprite != null) _targetImage.sprite = keyboardSprite;
        }
    }
}