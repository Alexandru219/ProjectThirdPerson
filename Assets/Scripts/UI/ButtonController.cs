using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerClickHandler, ISubmitHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySound("ClickBtn");
    }

    public void OnSubmit(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySound("ClickBtn");
    }
}
