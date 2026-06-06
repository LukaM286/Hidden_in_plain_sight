using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using TMPro;

public class InsanityManager : MonoBehaviour
{
    [Header("Insanity")]
    public float maxInsanity = 100f;
    public float currentInsanity = 0f;
    public float insanityRate = 2f; 
    
    [Header("UI")]
    public Slider insanityBar;
    
    [Header("Game Over")]
    public GameOverUI gameOverUI;
    
    [Header("Visual Effects")]
    public Image vignetteOverlay; 
    
    private bool isDead = false;

    void Start()
    {
        currentInsanity = 0f;
        if (insanityBar != null)
        {
            insanityBar.maxValue = maxInsanity;
            insanityBar.value = 0f;
        }
    }

    void Update()
    {
        if (isDead) return;

        currentInsanity += insanityRate * Time.deltaTime;
        currentInsanity = Mathf.Clamp(currentInsanity, 0f, maxInsanity);

        if (insanityBar != null)
            insanityBar.value = currentInsanity;

        // vignette efekt
        if (vignetteOverlay != null)
        {
            float alpha = currentInsanity / maxInsanity * 1f;
            vignetteOverlay.color = new Color(0f, 0f, 0f, alpha);
        }

        if (currentInsanity >= maxInsanity)
        {
            isDead = true;
            float alpha = 0;
            vignetteOverlay.color = new Color(0f, 0f, 0f, alpha);
            gameOverUI.Show();
        }
    }

    public void ReduceInsanity(float amount)
    {
        currentInsanity -= amount;
        currentInsanity = Mathf.Clamp(currentInsanity, 0f, maxInsanity);
    }
}