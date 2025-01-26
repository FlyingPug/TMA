using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    private string fullText;
    private Coroutine typingCoroutine;


    public void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    public void ShowText(string text, float delay)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        fullText = text;
        textMeshPro.text = "";
        typingCoroutine = StartCoroutine(TypeText(delay));
    }

    private IEnumerator TypeText(float delay)
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            textMeshPro.text = fullText.Substring(0, i);
            yield return new WaitForSeconds(delay);
        }
    }
}
