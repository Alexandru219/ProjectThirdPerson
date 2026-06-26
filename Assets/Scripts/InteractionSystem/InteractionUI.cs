using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using Zenject;

public class InteractionUI : MonoBehaviour
{
  //  [Inject]
    [SerializeField] private InteractionModule _interactionModule;
    [SerializeField] private TMP_Text interactionMessage;
    [SerializeField] private Image interactionProgressImage;
    [SerializeField] private Transform _rootInteractionUI;

    private void Awake()
    {
       // _rootInteractionUI = GetComponentInChildren<Transform>();
    }

    public void Start()
    {
        _interactionModule.onInteractableHovered.AddListener(HandleInteractableFocused);
        _interactionModule.onInteractProgressChanged.AddListener(HandleFillProgress);
        _rootInteractionUI.gameObject.SetActive(false);
    }

    private void HandleFillProgress(float arg0)
    {
        interactionProgressImage.fillAmount = arg0;
    }

    private void HandleInteractableFocused(string message)
    {
        if (message != "")
        {
            _rootInteractionUI.gameObject.SetActive(true);
            interactionMessage.text = message;
        }
        else
        {
            _rootInteractionUI.gameObject.SetActive(false);
        }
    }
}
