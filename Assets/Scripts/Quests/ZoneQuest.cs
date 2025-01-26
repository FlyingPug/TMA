using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneQuest : MonoBehaviour
{
    [Header("Quest Settings")]
    public QuestManager questManager;
    public int completedQuestId;
    public int nextQuestId;    
    public List<int> subtitleIds; 

    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            StartCoroutine(HandleZoneQuest());
        }
    }

    private IEnumerator HandleZoneQuest()
    {
        foreach (int subtitleId in subtitleIds)
        {
            questManager.subtitleManager.ShowSubtitleText(questManager.allQuests[subtitleId].subtitleText);
            yield return new WaitForSeconds(2.0f);
        }

        questManager.CompleteQuest(completedQuestId);
        questManager.AddQuest(nextQuestId);
    }
}