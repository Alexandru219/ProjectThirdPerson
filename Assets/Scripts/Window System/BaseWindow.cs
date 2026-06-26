using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class BaseWindow : MonoBehaviour
{
    private GameObject firstSelected;
    private CanvasGroup canvasGroup;

    public GameObject FirstSelected
    {
        get { return firstSelected; } 
        set { firstSelected = value; }
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

   

    private void OnDisable()
    {
        
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
        SetFocus(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }

    

    public void SetFocus(bool isFocused)
    {
        canvasGroup.interactable = isFocused;
        canvasGroup.blocksRaycasts = isFocused; 

        if (isFocused && firstSelected != null && InputManager.Instance.IsGamepad)
        {
            if(firstSelected != null) firstSelected = GetComponentInChildren<Button>().gameObject;
            
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }
    }
    
  
}