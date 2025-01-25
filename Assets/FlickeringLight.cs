using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light lampLight;
    public float flickerSpeed = 0.1f;
    public float flickerIntensity = 0.5f;
    public float maxIntensity = 1f;

    private bool isFlickering = true;
    private float timer = 0f;

    void Start()
    {
        if (lampLight == null)
        {
            lampLight = GetComponent<Light>();
        }
    }

    void Update()
    {
        if (isFlickering)
        {
            timer += Time.deltaTime * flickerSpeed;
            lampLight.intensity = Mathf.Lerp(maxIntensity - flickerIntensity, maxIntensity, Mathf.PerlinNoise(timer, 0f));
        }
    }

    public void ToggleFlickering(bool enable)
    {
        isFlickering = enable;
    }
}
