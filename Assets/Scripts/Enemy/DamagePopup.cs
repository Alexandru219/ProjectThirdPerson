using System.Collections;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [Header("Referință text")]
    [SerializeField] private TextMeshProUGUI damageText;  

    [Header("Animație")]
    [SerializeField] private float moveSpeed  = 1.5f;  
    [SerializeField] private float duration   = 0.9f;  

    private Vector3 startLocalPos;
    private Coroutine activeCoroutine;

    private void Awake()
    {
        startLocalPos = transform.localPosition;
        gameObject.SetActive(false); 
    }

    public void Show(float damage)
    {
        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        damageText.text = Mathf.RoundToInt(damage).ToString();

        transform.localPosition = startLocalPos;
        gameObject.SetActive(true);

        activeCoroutine = StartCoroutine(Animate());
    }

    // ─────────────────────────────────────────────
    private IEnumerator Animate()
    {
        float elapsed = 0f;
        Color originalColor = damageText.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;  

            transform.localPosition = startLocalPos + Vector3.up * moveSpeed * elapsed;

            float alpha = t < 0.6f ? 1f : Mathf.Lerp(1f, 0f, (t - 0.6f) / 0.4f);
            damageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            if (Camera.main != null)
                transform.parent.rotation = Camera.main.transform.rotation;

            yield return null;
        }

        damageText.color = originalColor;
        transform.localPosition = startLocalPos;
        gameObject.SetActive(false);
    }
}