using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public string message = "Interact";
    [SerializeField] private bool interactionEnabled = true;

    public bool InteractionEnabled
    {
        get => interactionEnabled;
        set
        {
            interactionEnabled = value;
            OnInteractableDisabled?.Invoke(this);
        }
    }

    [SerializeField, Range(0f, 10f)] public float holdDuration = 0f;
    
    private float _interactionProgress;
   // private Outline _outline;

    public UnityEvent onInteract;
    public UnityEvent onHoverStarted;
    public UnityEvent onHoverEnded;
    public UnityAction<Interactable> OnInteractableDisabled;
    
    protected void Start()
    {
        /*_outline = GetComponent<Outline>();
        if (_outline)
        {
            onHoverStarted.AddListener(() => _outline.enabled = true);
            onHoverEnded.AddListener(() => _outline.enabled = false);
            _outline.enabled = false;
        }*/
    }

    public virtual void HoverStart()
    {
        if (!interactionEnabled)
            return;
        onHoverStarted.Invoke();
    }

    public virtual void HoverEnd()
    {
        if (!interactionEnabled)
            return;
        onHoverEnded.Invoke();
    }

    public virtual void Interact()
    {
        if (interactionEnabled)
        {
            onInteract?.Invoke();
        } 
    }
    public virtual bool CheckInteractable()
    {
        return true;
    }
}