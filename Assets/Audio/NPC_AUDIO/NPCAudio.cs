using UnityEngine;

public class NPCAudio : MonoBehaviour
{
    public AudioClip[] talkingSounds;
    public float minInterval = 3f;
    public float maxInterval = 8f;

    private AudioSource audioSource;
    private float timer;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f; // 3D zvok
        audioSource.maxDistance = 10f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        
        timer = Random.Range(minInterval, maxInterval);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            PlayRandomSound();
            timer = Random.Range(minInterval, maxInterval);
        }
    }

    void PlayRandomSound()
    {
        if (talkingSounds.Length == 0) return;
        AudioClip clip = talkingSounds[Random.Range(0, talkingSounds.Length)];
        audioSource.PlayOneShot(clip);
    }
}