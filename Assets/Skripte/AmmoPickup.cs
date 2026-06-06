using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 5;
    public AudioClip pickupSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Gun gun = other.GetComponentInChildren<Gun>();
            if (gun != null)
            {
                gun.ammo += ammoAmount;
                gun.UpdateAmmoUI();
                if (pickupSound != null)
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                Destroy(gameObject);
            }
        }
    }
}