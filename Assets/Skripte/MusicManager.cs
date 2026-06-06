using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleMusic(bool isOn)
    {
        Debug.Log("ToggleMusic called, isOn: " + isOn);
        AudioSource audio = GetComponent<AudioSource>();
        if (isOn) audio.Play();
        else audio.Stop();
    }
}