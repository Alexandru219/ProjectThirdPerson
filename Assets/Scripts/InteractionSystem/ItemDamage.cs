using UnityEngine;

public class ItemDamage : Interactable
{
    [SerializeField] private float rotationSpeed = 90f;

    private new void Start()
    {
        base.Start();
        onInteract.AddListener(InteractTool);
    }
    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);
    }

    private void InteractTool()
    {
        if (PlayerAttack.Instance != null)
        {
            if (PlayerUiDataController.Instance.BalanceValue >= 5)
            {
                Debug.Log("increase damage");
                PlayerAttack.Instance.IncreaseDamage();
                PlayerUiDataController.Instance.DecreaseBalance(5);
                SaveManager.Instance.SaveGame();

                gameObject.SetActive(false);
            }else
            {
                TutorialManager.Instance.ShowAlert("NoBalance");
            }
        }
    }
}
