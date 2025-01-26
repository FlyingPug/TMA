using System.Collections;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public bool isIndoors;
    public GameObject snowfallPrefab;
    public AudioClip indoorsClip;
    public AudioClip outdoorsClip;
    private Fader fader; 
    public PlayerMovement playerController;

    private AudioSource audioSource;
    private Renderer snowfallRenderer;
    private Fog fogComponent;
    private bool currentState;

    public float fadeDuration = 5f;

    private void Start()
    {   
        fader = GetComponent<Fader>();

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

    public void OnValidate()
    {
        if (Application.isPlaying && currentState != isIndoors)
        {
            UpdateEnvironment(isIndoors);
        }
    }

    private void UpdateEnvironment(bool indoors)
    {
        StopAllCoroutines();
        StartCoroutine(HandleWeatherTransition(indoors));
    }

    private IEnumerator HandleWeatherTransition(bool indoors)
    {
        playerController.enabled = false;

        fader.fadeState = Fader.FadeState.Out;
        fader.fadeSpeed = 1f / fadeDuration;
        yield return new WaitUntil(() => fader.fadeState == Fader.FadeState.OutEnd);

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

        fader.fadeState = Fader.FadeState.In;
        fader.fadeSpeed = 1f / fadeDuration;
        yield return new WaitUntil(() => fader.fadeState == Fader.FadeState.InEnd);

        fader.fadeState = Fader.FadeState.Out;
        yield return new WaitUntil(() => fader.fadeState == Fader.FadeState.OutEnd);

        playerController.enabled = true;
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
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, maxVolume, elapsed / fadeDuration);
            yield return null;
        }

        audioSource.volume = maxVolume;
    }

    private IEnumerator FadeOutSound()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, elapsed / fadeDuration);
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
