using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 10f;
    public YouWinUI youWinUI;
    public bool isNPC = false;
    public float insanityPenalty = 30f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log(gameObject.name + " took " + amount + " damage. Remaining health: " + health);

        if (health <= 0f)
            Die();
    }

    void Die()
    {
        if (isNPC)
        {
            InsanityManager im = FindObjectOfType<InsanityManager>();
            if (im != null)
                im.currentInsanity += insanityPenalty;
        }
        else
        {
            Debug.Log("Die called, youWinUI is: " + (youWinUI == null ? "NULL" : "connected"));
            if (youWinUI != null)
                youWinUI.Show();
        }

        Destroy(gameObject);
    }
}