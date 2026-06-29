using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack Instance;
    [Header("Attack Elements")]
    [SerializeField] private float damage  = 25f;
    [SerializeField] private float critChance = 0.2f;   // 20%
    [SerializeField] private float critMultiplier = 2f;
    [SerializeField] private float attackRange = 20f;

   // [SerializeField] private float damage = 5f; // ±5

    [Header("Raycast")]
    [SerializeField] private LayerMask enemyLayer;  
    private Camera mainCam;
    
    

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mainCam = Camera.main;
    }

    public void IncreaseDamage()
    {
        damage += damage * 0.07f;
      //  SaveManager.Instance.SaveGame();
    }

    public void DecreaseStamina()
    {
        PlayerUiDataController.Instance.UpdateStamina(5);
    }
   
    public void TryAttack()
    {
        Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, attackRange, enemyLayer))
        {
            Enemy enemy = hit.collider.GetComponentInParent<Enemy>();
            if (enemy == null) return;

            enemy.TakeDamage(damage);
            PlayerUiDataController.Instance.IncreaseBalance(1);

            Debug.Log($"[PlayerAttack] Damage: {damage}");
        }
        else
        {
            Debug.Log("[PlayerAttack] No Damage");
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (mainCam == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(mainCam.transform.position,
                       mainCam.transform.forward * attackRange);
    }
}
