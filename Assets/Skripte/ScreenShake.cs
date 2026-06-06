using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public InsanityManager insanityManager;
    public float shakeIntensity = 0.1f;
    public AudioSource audioSource;
    public AudioClip screamSound;
    public float screamThreshold = 0.85f;

    private Vector3 originalPos;
    private bool hasScreened = false;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        if (insanityManager == null) return;

        float insanityPercent = insanityManager.currentInsanity / insanityManager.maxInsanity;

        if (insanityPercent > 0.65f)
        {
            float intensity = shakeIntensity * (insanityPercent - 0.65f) * 4f;
            transform.localPosition = originalPos + Random.insideUnitSphere * intensity;
        }
        else
        {
            transform.localPosition = originalPos;
            //hasScreened = false;
        }

        if (insanityPercent > screamThreshold && !hasScreened)
        {
            hasScreened = true;
            AudioSource.PlayClipAtPoint(screamSound, transform.position);
        }
        else if (insanityPercent < screamThreshold)
        {
            hasScreened = false;
        }
    }
}