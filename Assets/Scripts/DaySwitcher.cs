using NUnit.Framework;
using System;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class DaySwitcher : MonoBehaviour
{
    struct DayScenario
    {
        public int id { get; set; }
        public WeatherType WeatherType { get; set; }
        public string Name { get; set; }
    }

    public SubtitleManager titleManager;
    public Transform wakeUpPosition;
    public FadeEffect playerFadeEffect;

    private List<DayScenario> dayScenarioList;

    void Start()
    {
        LoadScript();
    }

    public void LoadScript()
    {
        string filePath = Path.Combine(Application.dataPath, "days_scenario.xml");
        Debug.Log($"Attempting to load file: {filePath}");

        if (File.Exists(filePath))
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(filePath);

                XmlNodeList lines = xmlDocument.GetElementsByTagName("Day");
                foreach (XmlNode line in lines)
                {
                    int id = int.Parse(line.Attributes["id"].Value);
                    string text = line.Attributes["text"].Value;
                    WeatherType weatherType = Enum.Parse<WeatherType>(line.Attributes["weatherType"].Value);

                    dayScenarioList.Add(new DayScenario
                    {
                        id = id,
                        Name = text,
                        WeatherType = weatherType
                    });
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

    public void StartDay(int id)
    {

    }
}
