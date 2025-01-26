using System.Collections;
using UnityEngine;

public class SoundZoneTrigger : MonoBehaviour
{
    public AudioClip indoorsClip;
    public AudioClip outdoorsClip;
    private AudioSource audioSource;
    private bool isIndoors = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = 0f;
        PlaySound(indoorsClip, 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isIndoors)
        {
            isIndoors = true;
            PlaySound(indoorsClip, 0.1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isIndoors)
        {
            isIndoors = false;
            PlaySound(outdoorsClip, 0.25f);
        }
    }

    private void PlaySound(AudioClip newClip, float maxVolume)
    {
        StopAllCoroutines();
        StartCoroutine(SwitchSound(newClip, maxVolume));
    }

    private IEnumerator SwitchSound(AudioClip newClip, float maxVolume)
    {
        if (audioSource.clip == newClip) yield break;
        yield return StartCoroutine(FadeOutSound());

        audioSource.clip = newClip;
        audioSource.Play();
        yield return StartCoroutine(FadeInSound(maxVolume));
    }

    private IEnumerator FadeInSound(float maxVolume)
    {
        float duration = 2f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, maxVolume, elapsed / duration);
            yield return null;
        }
        audioSource.volume = maxVolume;
    }

    private IEnumerator FadeOutSound()
    {
        float duration = 2f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, elapsed / duration);
            yield return null;
        }
        audioSource.volume = 0f;
    }
}