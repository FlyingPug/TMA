using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeEffect : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;

    void Start()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        FadeOut();
    }

    public void FadeIn()
    {
        StartCoroutine(FadeCanvasGroup(0f, 1f));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCanvasGroup(1f, 0f));
    }

    private IEnumerator FadeCanvasGroup(float startAlpha, float endAlpha)
    {
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}
