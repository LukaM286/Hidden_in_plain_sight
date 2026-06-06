using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public int weaponIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        WeaponSwitching ws = other.GetComponentInChildren<WeaponSwitching>();
        if (ws == null) return;

        ws.UnlockWeapon(weaponIndex);

        Transform weaponTransform = ws.transform.GetChild(weaponIndex);
        weaponTransform.gameObject.SetActive(true);

        Gun gunScript = weaponTransform.GetComponent<Gun>();
        if (gunScript != null) gunScript.OnPickup();

        Knife knifeScript = weaponTransform.GetComponent<Knife>();
        if (knifeScript != null) knifeScript.OnPickup();

        Destroy(gameObject);
    }
}