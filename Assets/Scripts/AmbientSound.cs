using UnityEngine;

public class AmbientSound : MonoBehaviour
{
    public AudioClip ambientClip;
    public float volume = 0.5f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = ambientClip;
        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.Play();
    }

    public void ChangeVolume(float newVolume)
    {
        audioSource.volume = newVolume;
    }

    public void StopSound()
    {
        audioSource.Stop();
    }

    public void PlaySound()
    {
        audioSource.Play();
    }
}
