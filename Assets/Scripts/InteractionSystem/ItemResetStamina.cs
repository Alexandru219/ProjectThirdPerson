using UnityEngine;

public class ItemResetStamina : Interactable
{
    [SerializeField] private float rotationSpeed = 90f;
    private new void Start()
    {
        base.Start();
        onInteract.AddListener(InteractItem);
    }
    
    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);
    }

    private void InteractItem()
    {
        if (PlayerUiDataController.Instance.BalanceValue >= 5)
        {
            PlayerUiDataController.Instance.ResetStamina();
            PlayerUiDataController.Instance.DecreaseBalance(5);
            SaveManager.Instance.SaveGame();
            gameObject.SetActive(false);
        }
        else
        {
            TutorialManager.Instance.ShowAlert("NoBalance");
        }
    }
}
