using UnityEngine;

public class InsanityPickup : MonoBehaviour
{
    public float reduceAmount = 25f;
    public AudioClip pickupSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InsanityManager im = FindObjectOfType<InsanityManager>();
            if (im != null)
            {
                im.ReduceInsanity(reduceAmount);
                if (pickupSound != null)
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                Destroy(gameObject);
            }
        }
    }
}