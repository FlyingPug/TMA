using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneQuest : MonoBehaviour
{
    [Header("Quest Settings")]
    public QuestManager questManager;
    public int completedQuestId;
    public int nextQuestId;
    public string comparedTag = "Player";
    public List<int> subtitleIds; 

    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(comparedTag) && !isTriggered)
        {
            isTriggered = true;
            StartCoroutine(HandleZoneQuest());
        }
    }

    private IEnumerator HandleZoneQuest()
    {
        Quest completedQuest = questManager.currentQuests.Find(quest => quest.id == completedQuestId);

        if (completedQuest == null || completedQuest.isCompleted)
            yield break;

        foreach (int subtitleId in subtitleIds)
        {
            questManager.subtitleManager.EnqueueSubtitle(subtitleId);
            yield return new WaitForSeconds(0.0f);
        }

        questManager.CompleteQuest(completedQuestId);
        questManager.AddQuest(nextQuestId);
    }
}