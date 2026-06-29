using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopupController : MonoBehaviour
{
    public string namePopup;
    [SerializeField] private Button _closeButton;

    

    private void OnEnable()
    {
        //TutorialManager.Instance.isPopupActive = true;
        EventSystem.current.SetSelectedGameObject(_closeButton.gameObject);
        InputReaderNew.Instance.EnableUI();
    }

    private void OnDisable()
    {
        TutorialManager.Instance.isPopupActive = false;
        InputReaderNew.Instance.EnableGameplay();
    }
}
