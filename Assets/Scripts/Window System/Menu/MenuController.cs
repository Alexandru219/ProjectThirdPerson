using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject _firstSelectable;

    private void Start()
    {
       // _firstSelectable = GetComponentInChildren<Selectable>().gameObject;

    }

    private void OnEnable()
    {
        WindowManager.Instance.UpdateCurrentMenu(this);
        if(_firstSelectable != null) _firstSelectable = GetComponentInChildren<Selectable>().gameObject;
        InputManager.Instance.LastHoveredElement = null;
        
        if ( TrackerDeviceManager.Instance.CurrentNavigateUIDevice == TrackerDeviceManager.DeviceNavigateUI.Keyboard
             || TrackerDeviceManager.Instance.CurrentDevice == TrackerDeviceManager.DeviceType.Gamepad)
        {
            //InputManager.Instance.LastHoveredElement = _firstSelectable;
            EventSystem.current.SetSelectedGameObject(_firstSelectable);

            StartCoroutine(delay());
        }
    }

    private IEnumerator delay()
    {
        yield return new WaitForSeconds(0.12f);
        if(EventSystem.current.currentSelectedGameObject == null) EventSystem.current.SetSelectedGameObject(_firstSelectable);

    }
    
    public void SelectFirstBtnMenu()
    {
        if (!InputReaderNew.Instance.IsUIActive) return;
        
       // GameObject firstSelectable = GetComponentInChildren<Selectable>().gameObject;

        if (EventSystem.current.currentSelectedGameObject != null) return;
        if (TrackerDeviceManager.Instance.CurrentDevice == TrackerDeviceManager.DeviceType.Gamepad || TrackerDeviceManager.Instance.CurrentNavigateUIDevice == TrackerDeviceManager.DeviceNavigateUI.Keyboard)
        {
            if (InputManager.Instance.LastHoveredElement != null)
            {
                EventSystem.current.SetSelectedGameObject(InputManager.Instance.LastHoveredElement);
            }else if (InputManager.Instance.LastHoveredElement == null)
            {
                EventSystem.current.SetSelectedGameObject(_firstSelectable);
            }
        }
        
    }
}
