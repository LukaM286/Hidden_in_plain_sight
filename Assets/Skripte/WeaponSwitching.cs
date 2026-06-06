using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;
    public bool[] unlockedWeapons;

    void Start()
    {
        unlockedWeapons = new bool[transform.childCount];

        // vse orožje na začetku locked
        for (int i = 0; i < unlockedWeapons.Length; i++)
            unlockedWeapons[i] = false;

        SelectWeapon();
    }

    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) DoScroll(1);
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) DoScroll(-1);

        if (Input.GetKeyDown(KeyCode.Alpha1) && unlockedWeapons.Length >= 1 && unlockedWeapons[0]) selectedWeapon = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2) && unlockedWeapons.Length >= 2 && unlockedWeapons[1]) selectedWeapon = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3) && unlockedWeapons.Length >= 3 && unlockedWeapons[2]) selectedWeapon = 2;

        if (previousSelectedWeapon != selectedWeapon)
            SelectWeapon();
    }

    void DoScroll(int direction)
    {
        int count = transform.childCount;
        int start = selectedWeapon;

        do
        {
            selectedWeapon = (selectedWeapon + direction + count) % count;
        } while (!unlockedWeapons[selectedWeapon] && selectedWeapon != start);
    }

    public void UnlockWeapon(int index)
    {
        unlockedWeapons[index] = true;

        // opcijsko: ob prvi odklenitvi avtomatsko izberi orožje
        selectedWeapon = index;
        SelectWeapon();
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon && unlockedWeapons[i])
            {
                weapon.gameObject.SetActive(true);

                // sproži animacijo EquipGun
                Animator anim = weapon.GetComponent<Animator>();
                if (anim != null)
                    anim.SetTrigger("Equip");
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
