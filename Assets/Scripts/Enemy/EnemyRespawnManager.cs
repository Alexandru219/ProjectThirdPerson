using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyRespawnManager : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();

    [SerializeField] private float respawnDelay         = 5f;  
    [SerializeField] private float healthIncreasePercent = 5f;

    private Dictionary<Enemy, int> respawnCount = new Dictionary<Enemy, int>();

    private void Start()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy == null) continue;
            enemy.OnDeath += HandleEnemyDeath;
            respawnCount[enemy] = 0;
        }
    }

    private void OnDestroy()
    {
    
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
                enemy.OnDeath -= HandleEnemyDeath;
        }
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        StartCoroutine(RespawnRoutine(enemy));
    }

    private IEnumerator RespawnRoutine(Enemy enemy)
    {
        yield return new WaitForSeconds(respawnDelay);

        if (enemy == null) yield break;   

        respawnCount[enemy]++;
        int count = respawnCount[enemy];
        float multiplier = Mathf.Pow(1f + healthIncreasePercent / 100f, count);
        float newMaxHealth = enemy.MaxHealth * (1f + healthIncreasePercent / 100f);

        Debug.Log($"[RespawnManager] Respawn #{count}. " +
                  $" MaxHP: {newMaxHealth:F1} (+{healthIncreasePercent}%)");

        enemy.Respawn(newMaxHealth);
    }

    public void RegisterEnemy(Enemy enemy)
    {
        if (enemies.Contains(enemy)) return;
        enemies.Add(enemy);
        respawnCount[enemy] = 0;
        enemy.OnDeath += HandleEnemyDeath;
    }
}