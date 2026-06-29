using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("UI Health Bar ")]
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private GameObject healthBarObject; 

    [Space]
    [SerializeField] private DamagePopup damagePopup; 

   
    [SerializeField] private GameObject deathEffectPrefab; 
    public event Action<Enemy> OnDeath;
    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
       // UpdateHealthBar();
    }

    
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        damage = Mathf.Max(0, damage);           
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthBar();
       // ShowDamagePopup(damage);
       // damagePopup?.Show(damage);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("[Enemy] is died");

        if (healthBarObject != null)
            healthBarObject.SetActive(false);

        if (deathEffectPrefab != null) Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

      //  gameObject.SetActive(false);
      StartCoroutine(DieRoutine());
    }
    private IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(0.1f);   
        OnDeath?.Invoke(this);                    
        gameObject.SetActive(false);              
    }
    private void UpdateHealthBar()
    {
        if (healthBarSlider == null) return;
        
        healthBarSlider.value = currentHealth;
    }
    public void Respawn(float newMaxHealth)
    {
        maxHealth      = newMaxHealth;
        currentHealth  = maxHealth;
        isDead         = false;
 
        if (healthBarObject != null)
            healthBarObject.SetActive(true);
 
        UpdateHealthBar();
        gameObject.SetActive(true);
 
        Debug.Log($"[Enemy]: {maxHealth:F1}");
    }
 

    public float CurrentHealth => currentHealth;
    public float MaxHealth  => maxHealth;
    public bool  IsDead  => isDead;
}
