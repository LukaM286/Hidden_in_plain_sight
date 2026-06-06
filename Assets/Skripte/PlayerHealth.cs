using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public Slider healthBar;
    public float barSmoothSpeed = 5f;

    [Header("Effects")]
    public Image damageOverlay;
    public float overlayFadeSpeed = 2f;

    [Header("Game Over")]
    public GameOverUI gameOverUI;

    [Header("Heal Effect")]
    public Image healOverlay;
    
    private float targetHealth;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        targetHealth = currentHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (damageOverlay != null)
        {
            damageOverlay.color = new Color(1, 0, 0, 0);
        }
    }

    void Update()
    {
        // Smooth HP bar
        if (healthBar != null)
        {
            healthBar.value = Mathf.Lerp(
                healthBar.value,
                targetHealth,
                Time.deltaTime * barSmoothSpeed
            );
        }

        // Fade damage overlay
        if (damageOverlay != null)
        {
            Color c = damageOverlay.color;
            c.a = Mathf.Lerp(c.a, 0f, Time.deltaTime * overlayFadeSpeed);
            damageOverlay.color = c;
        }

        if (healOverlay != null){
            Color c = healOverlay.color;
            c.a = Mathf.Lerp(c.a, 0f, Time.deltaTime * overlayFadeSpeed);
            healOverlay.color = c;
        }
    }

    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        targetHealth = currentHealth;

        if (damageOverlay != null)
        {
            damageOverlay.color = new Color(1, 0, 0, 0.4f);
        }

        Debug.Log("PLAYER HIT! HP = " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        targetHealth = currentHealth;
        
        if (healOverlay != null)
            healOverlay.color = new Color(0, 1, 0, 0.4f); // zelena
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("PLAYER DEAD");
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        if (gameOverUI != null)
        {
            gameOverUI.Show();
        }

        // počakamo frame v realnem času (ni vezano na Time.timeScale)
        yield return new WaitForSecondsRealtime(0.1f);

        Time.timeScale = 0f;
    }
}
