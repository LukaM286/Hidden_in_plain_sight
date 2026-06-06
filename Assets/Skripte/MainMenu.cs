using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public Toggle musicToggle;

    void Start()
    {
        settingsPanel.SetActive(false);
        musicToggle.isOn = true;
        musicToggle.onValueChanged.AddListener(ToggleMusic);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("LoadingScreen");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QUIT");
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    void ToggleMusic(bool isOn)
    {
        MusicManager mm = FindObjectOfType<MusicManager>();
        if (mm != null) mm.ToggleMusic(isOn);
    }
}