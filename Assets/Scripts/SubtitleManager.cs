using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using TMPro;

public class SubtitleManager : MonoBehaviour
{
    public string defaultLanguage = "en";
    private Dictionary<int, SubtitleEntry> subtitles = new Dictionary<int, SubtitleEntry>();

    public TMP_Text subtitleText;
    public CanvasGroup panelCanvasGroup;
    public float defaultDisplayTime = 2f;
    public float fadeDuration = 0.5f;

    private Queue<SubtitleData> subtitleQueue = new Queue<SubtitleData>();
    private bool isDisplaying = false;

    void Start()
    {
        LoadLanguage(defaultLanguage);
    }

    public void LoadLanguage(string languageCode)
    {
        subtitles.Clear();

        string filePath = Path.Combine(Application.dataPath, "Translations", $"subtitles_{languageCode}.xml");
        if (!File.Exists(filePath))
        {
            Debug.LogError($"XML file for language {languageCode} not found. Loading default language.");
            filePath = Path.Combine(Application.dataPath, "Translations", $"subtitles_{defaultLanguage}.xml");
        }

        if (File.Exists(filePath))
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(filePath);

                XmlNodeList lines = xmlDocument.GetElementsByTagName("Line");
                foreach (XmlNode line in lines)
                {
                    int id = int.Parse(line.Attributes["id"].Value);
                    string text = line.Attributes["text"].Value;
                    float displayTime = line.Attributes["displayTime"] != null
                        ? float.Parse(line.Attributes["displayTime"].Value)
                        : defaultDisplayTime;

                    subtitles[id] = new SubtitleEntry { Text = text, DisplayTime = displayTime };
                }

                Debug.Log("Subtitles loaded successfully.");
            }
            catch (XmlException e)
            {
                Debug.LogError($"Error parsing XML file: {e.Message}");
            }
        }
    }

    public SubtitleEntry GetSubtitle(int id)
    {
        return subtitles.ContainsKey(id) ? subtitles[id] : new SubtitleEntry { Text = $"[Subtitle #{id} not found]", DisplayTime = defaultDisplayTime };
    }

    public void EnqueueSubtitle(int id, bool fadeIn = true, bool fadeOut = true)
    {
        SubtitleEntry subtitle = GetSubtitle(id);
        EnqueueSubtitleText(subtitle.Text, fadeIn, fadeOut, subtitle.DisplayTime);
    }

    public void EnqueueSubtitleText(string text, bool fadeIn = true, bool fadeOut = true, float displayTime = 0)
    {
        subtitleQueue.Enqueue(new SubtitleData { Text = text, FadeIn = fadeIn, FadeOut = fadeOut, DisplayTime = displayTime });
        if (!isDisplaying)
            StartCoroutine(ProcessQueue());
    }

    private System.Collections.IEnumerator ProcessQueue()
    {
        isDisplaying = true;

        while (subtitleQueue.Count > 0)
        {
            SubtitleData currentSubtitle = subtitleQueue.Dequeue();
            yield return ShowSubtitleInternal(currentSubtitle.Text, currentSubtitle.FadeIn, currentSubtitle.FadeOut, currentSubtitle.DisplayTime);
        }

        isDisplaying = false;
    }

    private System.Collections.IEnumerator ShowSubtitleInternal(string text, bool fadeIn, bool fadeOut, float displayTime)
    {
        subtitleText.text = text;

        if (fadeIn)
            yield return StartCoroutine(FadeIn());
        else
            panelCanvasGroup.alpha = 1;

        yield return new WaitForSeconds(displayTime);

        if (fadeOut)
            yield return StartCoroutine(FadeOut());
        else
        {
            panelCanvasGroup.alpha = 0;
            subtitleText.text = "";
        }
    }

    private System.Collections.IEnumerator FadeIn()
    {
        panelCanvasGroup.alpha = 0;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            panelCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        panelCanvasGroup.alpha = 1;
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            panelCanvasGroup.alpha = Mathf.Clamp01(1 - (elapsed / fadeDuration));
            yield return null;
        }

        panelCanvasGroup.alpha = 0;
        subtitleText.text = "";
    }

    public class SubtitleEntry
    {
        public string Text;
        public float DisplayTime;
    }

    private class SubtitleData
    {
        public string Text;
        public bool FadeIn;
        public bool FadeOut;
        public float DisplayTime;
    }
}
