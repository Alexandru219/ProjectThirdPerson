using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [SerializeField] private List<AlertController> alerts;
    [SerializeField] private List<PopupController> popups;

    public bool isPopupActive;

    private void Awake()
    {
        Instance = this;
    }
    
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => !PauseMenuController.Instance.isPause);

        yield return new WaitForSeconds(1.5f);

        if (PauseMenuController.Instance.isPause)
        {
            yield return new WaitUntil(() => !PauseMenuController.Instance.isPause);
        }

        ShowPopup("startPopup");
    }

    public void ShowPopup(string popupName)
    {
        PopupController popup = GetPopupByName(popupName);

        if (popup != null)
        {
           popup.gameObject.SetActive(true);
           isPopupActive = true;
        }
        else
        {
            Debug.LogWarning("Alert not found: " + popupName);
        } 
    }

    public void ShowAlert(string alertName)
    {
        AlertController alert = GetAlertByName(alertName);

        if (alert != null)
        {
            StartCoroutine(DelayAlert(alert));
        }
        else
        {
            Debug.LogWarning("Alert not found: " + alertName);
        }
    }

    private AlertController GetAlertByName(string alertName)
    {
        return alerts.Find(a => a.nameAlert == alertName);
    }

    private IEnumerator DelayAlert(AlertController alert)
    {
        alert.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        alert.gameObject.SetActive(false);
    }
    
    private PopupController GetPopupByName(string popupName)
    {
        return popups.Find(a => a.namePopup == popupName);
    }

  
}