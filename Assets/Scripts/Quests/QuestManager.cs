using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    public TypewriterEffect currentQuestOutput;

    void Start()
    {
        subtitleManager = GetComponent<SubtitleManager>();
        LoadQuests();
        StartCoroutine(AddQuestWithDelay(1));
    }

    private void LoadQuests()
    {
        string filePath = Path.Combine(Application.dataPath, "Translations", "quests.xml");
        Debug.Log($"Attempting to load file: {filePath}");

        if (File.Exists(filePath))
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(filePath);

                XmlNodeList lines = xmlDocument.GetElementsByTagName("Quest");
                foreach (XmlNode line in lines)
                {
                    int id = int.Parse(line.Attributes["id"].Value);
                    string name = line.Attributes["name"].Value;
                    string description = line.Attributes["description"].Value;
                    string subtitleText = line.Attributes["subtitleText"].Value;

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

    public void AddQuest(int id)
    {
        Debug.Log($"trying to add quest {id}");
        Quest completedQuest = currentQuests.Find(quest => quest.id == id);

        if (completedQuest != null && completedQuest.isCompleted)
            return;
        Debug.Log($"Added quest {id}");
        StartCoroutine(AddQuestWithDelay(id));
    }

    private IEnumerator AddQuestWithDelay(int id)
    {
        if (allQuests.TryGetValue(id, out Quest quest))
        {
            if (quest.subtitleText.Length > 0)
            {
                subtitleManager.EnqueueSubtitleText(quest.subtitleText);
            }
            yield return new WaitUntil(() => subtitleManager.QueueLength == 0);
            // Добавляем квест в список
            currentQuests.Add(quest);
            UpdateUi();
        }
        else
        {
            Debug.LogError($"Quest with ID {id} not found!");
        }
    }

    public void CompleteQuest(int questId)
    {
        Quest quest = currentQuests.Find(q => q.id == questId);
        if (quest != null && !quest.isCompleted)
        {
            quest.CompleteQuest();
            subtitleManager.EnqueueSubtitleText(quest.subtitleText);
            Debug.Log($"Quest {quest.name} completed");
        }
    }

    public bool IsQuestCompleted(int questId)
    {
        Quest quest = currentQuests.Find(q => q.id == questId);
        return quest != null && quest.isCompleted;
    }

    private void UpdateUi()
    {
        StringBuilder task = new();

        foreach (Quest quest in currentQuests.Where(quest => !quest.isCompleted))
        {
            task.AppendLine(quest.description);
        }

        currentQuestOutput.ShowText(task.ToString(), 0.05f);
    }
}