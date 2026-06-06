using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float timeLimit = 180f; 
    public TMP_Text timerText;
    public GameOverUI gameOverUI;

    public float currentTime;
    private bool timerRunning = false;

    void Start()
    {
        currentTime = timeLimit;
        timerRunning = false;
    }

    public void StartTimer()
    {
        timerRunning = true;
    }

    void Update()
    {
        if (!timerRunning) return;

        currentTime -= Time.deltaTime;
        currentTime = Mathf.Max(0, currentTime);

        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);

        if (currentTime <= 0f)
        {
            timerRunning = false;
            gameOverUI.Show();
        }
    }
}