using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public RectTransform loadingCircle;
    public float rotationSpeed = 200f;
    public GameObject loadingPanel;
    public GameObject instructionsPanel;
    public Button startButton;

    void Start()
    {
        instructionsPanel.SetActive(false);
        StartCoroutine(LoadGame());
    }

    void Update()
    {
        if (loadingCircle != null)
            loadingCircle.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
    }

    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(2f);
        loadingPanel.SetActive(false);
        instructionsPanel.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}