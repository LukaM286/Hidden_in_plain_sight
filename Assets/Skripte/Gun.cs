using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    public float damage = 25f;   
    public float range = 100f;
    public Camera fpsCam;
    public int ammo = 5;
    public TMP_Text ammoText;
    public ParticleSystem muzzleFlash;
    public AudioSource audioSource;
    public AudioClip gunSound;

    [HideInInspector] public bool isPickedUp = false;

    void Update()
    {
        if (!isPickedUp) return;
        if (Input.GetButtonDown("Fire1")) Shoot();
        if (Input.GetKeyDown(KeyCode.F)) muzzleFlash.Play();
    }

    public void Shoot()
    {
        if (ammo <= 0) return;
        ammo--;
        UpdateAmmoUI();
        muzzleFlash.Play();
        audioSource.PlayOneShot(gunSound);

        Animator anim = GetComponent<Animator>();
        if (anim != null) anim.Play("GunRecoil", 0, 0f);
        
        int layerMask = ~LayerMask.GetMask("Player");
        
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position + fpsCam.transform.forward * 1.5f, fpsCam.transform.forward, out hit, range, layerMask))
        {
            Debug.Log("Player ray hit: " + hit.transform.name);
            Target target = hit.transform.GetComponentInParent<Target>();
            if (target != null) target.TakeDamage(damage);
        }
    }

    public void UpdateAmmoUI()
    {
        ammoText.text = isPickedUp ? $"AMMO: {ammo}" : "";
    }

    public void OnPickup()
    {
        isPickedUp = true;
        UpdateAmmoUI();
        Animator anim = GetComponent<Animator>();
        if (anim != null) anim.Play("EquipGun", 0, 0f);
    }
}