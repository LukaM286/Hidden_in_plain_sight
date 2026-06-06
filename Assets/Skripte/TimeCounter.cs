using UnityEngine;
using TMPro;
using System.Collections;

public class TimeCounter : MonoBehaviour
{
    public int timePlayed = 0;
    public TMP_Text timeText; // UI text (HUD)

    private Coroutine counterRoutine;

    void Start()
    {
        counterRoutine = StartCoroutine(GameCounter());
    }

    IEnumerator GameCounter()
    {
        WaitForSeconds wfs = new WaitForSeconds(1f);

        while (true)
        {
            timeText.text = timePlayed.ToString();
            yield return wfs;
            timePlayed++;
        }
    }

    public void StopCounter()
    {
        if (counterRoutine != null)
            StopCoroutine(counterRoutine);
    }

    public int GetTimePlayed()
    {
        return timePlayed;
    }
}
