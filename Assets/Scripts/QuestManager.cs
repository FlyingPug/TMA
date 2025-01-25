using System;
using System.Collections.Generic;
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
    public List<Quest> quests = new List<Quest>();
    public SubtitleManager subtitleManager;

    void Start()
    {
        subtitleManager = GetComponent<SubtitleManager>();
    }

    public void AddQuest(int id, string name, string description, string subtitleText)
    {
        quests.Add(new Quest(id, name, description, subtitleText));
    }

    public void CompleteQuest(int questId)
    {
        Quest quest = quests.Find(q => q.id == questId);
        if (quest != null && !quest.isCompleted)
        {
            quest.CompleteQuest();
            subtitleManager.ShowSubtitleText(quest.subtitleText);
            Debug.Log($"Quest {quest.name} completed");
        }
    }

    public bool IsQuestCompleted(int questId)
    {
        Quest quest = quests.Find(q => q.id == questId);
        return quest != null && quest.isCompleted;
    }
}
