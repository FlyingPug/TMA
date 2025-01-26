using System.Collections;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    [SerializeField] private bool isIndoors;
    public GameObject snowfallPrefab;
    public AudioClip indoorsClip;
    public AudioClip outdoorsClip;

    private AudioSource audioSource;
    private Renderer snowfallRenderer;
    private Fog fogComponent;
    private bool currentState;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = 0f;

        if (snowfallPrefab != null)
        {
            snowfallRenderer = snowfallPrefab.GetComponent<Renderer>();
        }

        fogComponent = Camera.main?.GetComponent<Fog>();

        UpdateEnvironment(isIndoors);
    }

    private void OnValidate()
    {
        if (Application.isPlaying && currentState != isIndoors)
        {
            UpdateEnvironment(isIndoors);
        }
    }

    private void UpdateEnvironment(bool indoors)
    {
        StopAllCoroutines();
        currentState = indoors;

        if (indoors)
        {
            StartCoroutine(SwitchSound(indoorsClip, 0.1f));
            ToggleSnowfall(false);
            ToggleFog(false);
        }
        else
        {
            StartCoroutine(SwitchSound(outdoorsClip, 0.25f));
            ToggleSnowfall(true);
            ToggleFog(true);
        }
    }

    private IEnumerator SwitchSound(AudioClip newClip, float maxVolume)
    {
        if (audioSource.clip == newClip) yield break;

        if (audioSource.isPlaying)
        {
            yield return StartCoroutine(FadeOutSound());
        }

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

    private void ToggleSnowfall(bool enable)
    {
        if (snowfallRenderer != null)
        {
            snowfallRenderer.enabled = enable;
        }
    }

    private void ToggleFog(bool enable)
    {
        if (fogComponent != null)
        {
            fogComponent.enabled = enable;
        }
    }
}
