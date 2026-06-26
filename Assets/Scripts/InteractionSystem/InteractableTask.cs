using UnityEngine;

public class InteractableTask : Interactable
{
    protected new void Start()
    {
        onInteract.AddListener(TaskInteract);
    }

    private void OnDestroy()
    {
        onInteract.RemoveListener(TaskInteract);
    }
    
    private void TaskInteract()
    {
        Debug.Log(gameObject.name);
    }   
}