using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class InteractionModule : MonoBehaviour
{
    public static InteractionModule Instance;

    // Settings
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField, Range(1f, 100f)] private float raycastDistance = 1f;

    // ReyCast
    private Transform _cameraTransform;
    private RaycastHit _raycastHit;
    private Collider _lastHitCollider;

    // Interaction
    private Interactable _currentInteractable;
    private float _interactionProgress;
    public UnityEvent<string> onInteractableHovered;
    public UnityEvent<float> onInteractProgressChanged;
    [SerializeField] private TMPro.TextMeshProUGUI uiInteractionText;
    [SerializeField] private GameObject interactionUI;
    private float InteractionProgress
    {
        get => _interactionProgress;
        set
        {
            _interactionProgress = value;
            onInteractProgressChanged?.Invoke(_interactionProgress);
        }
    }

    private Interactable CurrentInteractable
    {
        get => _currentInteractable;
        set
        {
            if (_currentInteractable)
            {
                _currentInteractable.HoverEnd();
                _currentInteractable.OnInteractableDisabled -= DisableInteractable;
            }

            _currentInteractable = value;

            if (_currentInteractable != null && _currentInteractable.InteractionEnabled)
            {
                if (_currentInteractable.CheckInteractable())
                {
                    _currentInteractable.HoverStart();
                    _currentInteractable.OnInteractableDisabled += DisableInteractable;
                    //onInteractableHovered?.Invoke(_currentInteractable.message);

                    uiInteractionText.text = _currentInteractable.GetLocalizedMessage();
                    uiInteractionText.gameObject.SetActive(true);
                    interactionUI.SetActive(true);
                }
                else
                {
                    onInteractableHovered?.Invoke("");
                }
            }else
            {
                onInteractableHovered?.Invoke("");
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);

        if (Physics.Raycast(ray, out _raycastHit, raycastDistance, interactableLayer))
        {
            Collider hitCollider = _raycastHit.collider;

            if (hitCollider != _lastHitCollider)
            {
                _lastHitCollider = hitCollider;
                CurrentInteractable = hitCollider.GetComponent<Interactable>();
                InteractionProgress = 0f;
            }
        }
        else if (_lastHitCollider != null)
        {
            _lastHitCollider = null;
            CurrentInteractable = null;
        }
    }

    public void StartInteraction()
    {
        if (CurrentInteractable == null) return;

        StopAllCoroutines();
        StartCoroutine(InteractionCoroutine());
    }

    public void StopInteraction()
    {
        StopAllCoroutines();
        InteractionProgress = 0f;
    }

    private IEnumerator InteractionCoroutine()
    {
        if (!CurrentInteractable)
        {
            yield break;
        }

        var duration = CurrentInteractable.holdDuration;
        var elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            InteractionProgress = elapsed / duration;
            yield return null;
        }

        CurrentInteractable.Interact();
        StopInteraction();
    }

    private void DisableInteractable(Interactable updatedInteractable)
    {
        CurrentInteractable = updatedInteractable;
    }
}