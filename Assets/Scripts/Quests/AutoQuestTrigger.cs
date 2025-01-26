using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoQuestTrigger : MonoBehaviour
{
    public QuestManager questManager;
    public int nextQuestId;
    public List<int> subtitleIds;
    public float startDelay = 3.0f;  

    private void Start()
    {
        StartCoroutine(HandleQuestFlow());
    }

    private IEnumerator HandleQuestFlow()
    {
        yield return new WaitForSeconds(startDelay); // ∆дЄм перед стартом

        foreach (int subtitleId in subtitleIds)
        {
            questManager.subtitleManager.EnqueueSubtitle(subtitleId);
            yield return new WaitUntil(() => questManager.subtitleManager.QueueLength == 0);
        }

        questManager.CompleteQuest(0);
        questManager.AddQuest(nextQuestId);
    }
}