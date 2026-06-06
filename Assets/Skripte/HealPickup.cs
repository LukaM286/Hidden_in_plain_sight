using UnityEngine;

public class HealPickup : MonoBehaviour
{
    public int healAmount = 25;
    public AudioClip pickupSound;


    void OnTriggerEnter(Collider other)
    {
        PlayerHealth hp = other.GetComponent<PlayerHealth>();
        if (hp != null)
        {
            hp.Heal(healAmount);
            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            Destroy(gameObject);
        }
    }
}
