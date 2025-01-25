using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using TMPro;

public class SubtitleManager : MonoBehaviour
{
    public string defaultLanguage = "en";
    private Dictionary<int, string> subtitles = new Dictionary<int, string>();

    public TMP_Text subtitleText;
    public float displayTime = 2f;

    private float timer = 0f;

    void Start()
    {
        LoadLanguage("en");
        ShowSubtitle(1);
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                subtitleText.text = "";
            }
        }
    }

    public void LoadLanguage(string languageCode)
    {
        subtitles.Clear();

        string filePath = Path.Combine(Application.dataPath, "Translations", $"subtitles_{languageCode}.xml");
        Debug.Log($"Attempting to load file: {filePath}");

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
                    subtitles[id] = text;
                }

                Debug.Log("Subtitles loaded successfully.");
            }
            catch (XmlException e)
            {
                Debug.LogError($"Error parsing XML file: {e.Message}");
            }
        }
        else
        {
            Debug.LogError("Failed to load the XML file.");
        }
    }

    public string GetSubtitle(int id)
    {
        return subtitles.ContainsKey(id) ? subtitles[id] : $"[Failed to find subtitle #{id}]";
    }

    public void ShowSubtitle(int id)
    {
        string subtitle = GetSubtitle(id);
        subtitleText.text = subtitle;
        timer = displayTime;
    }
}
