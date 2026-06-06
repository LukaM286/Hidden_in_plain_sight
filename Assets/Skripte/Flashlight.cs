using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public GameObject ON;            // GameObject, ki se pokaže, ko je flashlight vklopljen
    public GameObject OFF;           // GameObject, ki se pokaže, ko je flashlight izklopljen
    public Animator anim;            // Animator, ki vsebuje animacijo flasha
    public string flashlightBool = "flashlightOn"; // ime bool spremenljivke v Animatorju

    private bool isON = false;

    void Start()
    {
        // Začetno stanje
        ON.SetActive(false);
        OFF.SetActive(true);

        if (anim != null)
            anim.SetBool(flashlightBool, isON);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            isON = !isON;

            // Aktiviraj/izklopi GameObjecte
            ON.SetActive(isON);
            OFF.SetActive(!isON);

            // Nastavi bool v Animatorju
            if (anim != null)
                anim.SetBool(flashlightBool, isON);
        }
    }
}
