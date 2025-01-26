using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class Quest
{
    public int id;
    public string name;
    public string description;
    public bool isCompleted;
    public string subtitleText;

    public Quest(int id, string name, string description, string subtitleText)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.isCompleted = false;
        this.subtitleText = subtitleText;
    }

    public void CompleteQuest()
    {
        isCompleted = true;
    }
}

public class QuestManager : MonoBehaviour
{
    public List<Quest> currentQuests = new();
    public Dictionary<int, Quest> allQuests = new();
    public SubtitleManager subtitleManager;

    void Start()
    {
        subtitleManager = GetComponent<SubtitleManager>();
        LoadQuests();
    }

    private void LoadQuests()
    {
        string filePath = Path.Combine(Application.dataPath, "quests.xml");
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
                    string name = line.Attributes["name"].Value;
                    string description = line.Attributes["description"].Value;
                    string subtitleText = line.Attributes["subtitleText"].Value;
                    WeatherType weatherType = Enum.Parse<WeatherType>(line.Attributes["weatherType"].Value);

                    allQuests.Add(id, new Quest(id, name, description, subtitleText));
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

    public void AddQuest(int id, string name, string description, string subtitleText)
    {
        currentQuests.Add(new Quest(id, name, description, subtitleText));
    }

    public void CompleteQuest(int questId)
    {
        Quest quest = currentQuests.Find(q => q.id == questId);
        if (quest != null && !quest.isCompleted)
        {
            quest.CompleteQuest();
            subtitleManager.ShowSubtitleText(quest.subtitleText);
            Debug.Log($"Quest {quest.name} completed");
        }
    }

    public bool IsQuestCompleted(int questId)
    {
        Quest quest = currentQuests.Find(q => q.id == questId);
        return quest != null && quest.isCompleted;
    }
}
