using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverTracker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDeselectHandler
{
    private Selectable _selectable;

    private void Awake()
    {
        _selectable = GetComponent<Selectable>();
    }
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);

        if (InputManager.Instance != null && !InputManager.Instance.IsGamepad)
        {
            InputManager.Instance.LastHoveredElement = gameObject;
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (InputManager.Instance != null && InputManager.Instance.LastHoveredElement == gameObject)
        {
            //InputManager.Instance.LastHoveredElement = null;
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if(_selectable != null) _selectable.OnPointerExit(null);
    }
}