using UnityEngine;
using System.Collections;

public class Knife : MonoBehaviour
{
    public float damage = 50f;
    public float range = 2f;
    public Camera fpsCam;
    public AudioSource audioSource;
    public AudioClip swingSound;

    [HideInInspector] public bool isPickedUp = false;

    void Update()
    {
        if (!isPickedUp) return;
        if (Input.GetButtonDown("Fire1")) Attack();
    }

    void Attack()
    {
        Debug.Log("Knife attack called");
        if (audioSource != null && swingSound != null)
            audioSource.PlayOneShot(swingSound);

        Animator anim = GetComponent<Animator>();
        if (anim != null) anim.SetTrigger("Attack");

        int layerMask = ~LayerMask.GetMask("Player");

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position + fpsCam.transform.forward * 0.5f, fpsCam.transform.forward, out hit, range, layerMask))
        {
            Debug.Log("Knife hit: " + hit.transform.name);
            Target target = hit.transform.GetComponentInParent<Target>();
            if (target != null) target.TakeDamage(damage);
        }
    }

    public void OnPickup()
    {
        isPickedUp = true;
        Animator anim = GetComponent<Animator>();
        if (anim != null) anim.Play("KnifeEquip", 0, 0f);
    }
}