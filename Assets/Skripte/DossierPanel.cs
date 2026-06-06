using UnityEngine;
using System.Collections;

public class DossierPanel : MonoBehaviour
{
    public float displayTime = 5f;
    public GameTimer gameTimer;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        Time.timeScale = 0f;
        StartCoroutine(ShowDossier());
    }

    IEnumerator ShowDossier()
    {
        yield return new WaitForSecondsRealtime(displayTime);
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (gameTimer != null)
            gameTimer.StartTimer();
    }
}