using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public bool isIndoors;
    public GameObject snowfallPrefab;
    private Renderer snowfallRenderer;
    private Fog fogComponent;
    private bool currentState;

    private void Start()
    {
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
        currentState = indoors;
        ToggleSnowfall(!indoors);
        ToggleFog(!indoors);
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