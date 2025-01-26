using UnityEngine;
using TMPro;
using System.Collections;

public class PowerCountdown : MonoBehaviour
{
    public TextMeshPro textMeshPro;
    public int startValue = 10;
    public float duration = 5f;

    private float elapsedTime = 0f;

    private void Start()
    {
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TextMeshPro>();
        }

        if (textMeshPro != null)
        {
            StartCoroutine(CountdownRoutine());
        }
        else
        {
            Debug.LogError("TextMeshPro component not assigned!");
        }
    }

    private System.Collections.IEnumerator CountdownRoutine()
    {
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime)
        {
            elapsedTime = Time.time - startTime;
            float remainingPercentage = Mathf.Lerp(startValue, 0, elapsedTime / duration);
            int displayValue = Mathf.CeilToInt(remainingPercentage);

            textMeshPro.text = $"Power Left - {displayValue}%";

            yield return null; 
        }

        textMeshPro.text = "Power Left - 0%";
    }
}