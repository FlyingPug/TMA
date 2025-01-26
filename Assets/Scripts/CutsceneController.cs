using System.Collections;
using UnityEngine;
using TMPro;

public class CutsceneController : MonoBehaviour
{
    public Transform cameraStartPoint;
    public Transform cameraTarget;
    public float cameraMoveDuration = 5f;
    public GameObject player;
    public ParticleSystem snowfallParticles;
    public Fog fogController;
    public float fogFadeDuration = 3f;
    public AudioClip cutsceneMusic;
    public float musicFadeDuration = 2f;

    public AudioSource cutsceneMusicSource;
    public TextMeshProUGUI endText;
    private bool isCutsceneActive = false;

    private void Start()
    {
        if (fogController == null)
        {
            Debug.LogError("FogController is not assigned.");
        }

        if (cameraStartPoint == null)
        {
            Debug.LogError("Camera Start Point is not assigned.");
        }

        if (cameraTarget == null)
        {
            Debug.LogError("Camera Target is not assigned.");
        }

        if (player == null)
        {
            Debug.LogError("Player object is not assigned.");
        }

        if (cutsceneMusic == null)
        {
            Debug.LogError("Cutscene music is not assigned.");
        }

        if (endText == null)
        {
            Debug.LogError("EndText object is not assigned.");
        }

        //StartCutscene();
    }

    public void StartCutscene()
    {
        if (cameraStartPoint != null)
        {
            Camera.main.transform.position = cameraStartPoint.position;
            Camera.main.transform.rotation = cameraStartPoint.rotation;
        }

        if (isCutsceneActive) return;
        isCutsceneActive = true;
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        if (cutsceneMusic != null && cutsceneMusicSource != null)
        {
            cutsceneMusicSource.clip = cutsceneMusic;
            cutsceneMusicSource.volume = 0f;
            cutsceneMusicSource.Play();
            StartCoroutine(FadeInMusic());
        }

        if (player != null)
        {
            var playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }
        }

        Transform cameraTransform = Camera.main.transform;
        Vector3 startPosition = cameraStartPoint.position;
        Quaternion startRotation = cameraStartPoint.rotation;
        Vector3 endPosition = cameraTarget.position;
        Quaternion endRotation = cameraTarget.rotation;

        float elapsedTime = 0f;
        while (elapsedTime < cameraMoveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / cameraMoveDuration;
            cameraTransform.position = Vector3.Lerp(startPosition, endPosition, t);
            cameraTransform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        cameraTransform.position = endPosition;
        cameraTransform.rotation = endRotation;

        if (snowfallParticles != null && snowfallParticles.isPlaying)
        {
            snowfallParticles.Stop();
        }

        if (fogController != null)
        {
            float initialFogDensity = fogController.fogDensity;
            float elapsedTimeFog = 0f;
            while (elapsedTimeFog < fogFadeDuration)
            {
                elapsedTimeFog += Time.deltaTime;
                fogController.fogDensity = Mathf.Lerp(initialFogDensity, 0f, elapsedTimeFog / fogFadeDuration);
                yield return null;
            }

            fogController.fogDensity = 0f;
        }

        if (endText != null)
        {
            endText.text = "This is the end, thank you for playing!";
        }

        isCutsceneActive = false;
    }

    private IEnumerator FadeInMusic()
    {
        float elapsedTime = 0f;
        while (elapsedTime < musicFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            cutsceneMusicSource.volume = Mathf.Lerp(0f, 1f, elapsedTime / musicFadeDuration);
            yield return null;
        }
        cutsceneMusicSource.volume = 1f;
    }
}
