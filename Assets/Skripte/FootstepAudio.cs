using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    public AudioClip[] footstepSounds;
    public float stepInterval = 0.5f;
    
    private AudioSource audioSource;
    private float stepTimer;
    private CharacterController controller;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.isGrounded && controller.velocity.magnitude > 1f)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

void PlayFootstep()
{
    Debug.Log("Footstep played, velocity: " + controller.velocity.magnitude);
    if (footstepSounds.Length == 0) return;
    AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
    audioSource.PlayOneShot(clip);
}
}