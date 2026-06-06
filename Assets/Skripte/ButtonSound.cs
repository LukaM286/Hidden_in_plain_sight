using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public AudioClip clickSound;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        if (clickSound != null)
            AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
    }
}