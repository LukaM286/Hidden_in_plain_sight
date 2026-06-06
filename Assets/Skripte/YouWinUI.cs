using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class YouWinUI : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text timeText;
    public GameTimer gameTimer;

    void Start()
    {
        panel.SetActive(false);
    }

    public void Show()
    {
        panel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;

        if (timeText != null && gameTimer != null)
        {
            float elapsed = gameTimer.timeLimit - gameTimer.currentTime;
            int minutes = Mathf.FloorToInt(elapsed / 60f);
            int seconds = Mathf.FloorToInt(elapsed % 60f);
            timeText.text = $"Time: {minutes}:{seconds:00}";
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
        {
            Application.Quit();
            Debug.Log("QUIT GAME");
        }
}