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

    [Header("Zone Item Requirements")]
    public int requiredItemsCount = 1;  // ���������� ���������, ������� ������ ���� � ����


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(comparedTag))
        {
            StartCoroutine(HandleZoneQuest());
        }
    }

    private IEnumerator HandleZoneQuest()
    {
        Quest completedQuest = questManager.currentQuests.Find(quest => quest.id == completedQuestId);

        if (completedQuest == null || completedQuest.isCompleted)
            yield break;

        // ��������, ��� � ���� ���� ������ ���������� ���������
        int currentItemsCount = CountItemsInZone();
        if (currentItemsCount < requiredItemsCount)
        {
            Debug.Log($"Not enough items in the zone. Required: {requiredItemsCount}, Found: {currentItemsCount}");
            yield break; // ��������� ���������� ������, ���� ������������ ���������
        }

        // ���������� ��������
        foreach (int subtitleId in subtitleIds)
        {
            questManager.subtitleManager.EnqueueSubtitle(subtitleId);
            yield return new WaitForSeconds(0.0f); // ����� �������� �� �������� ��������� ���������, ���� ����������
        }

        // ��������� ������� ����� � ��������� ���������
        questManager.CompleteQuest(completedQuestId);
        questManager.AddQuest(nextQuestId);
    }

    // ����� ��� �������� ���������� ��������� � ���� (���������� ������)
    private int CountItemsInZone()
    {
        int itemCount = 0;
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10f); // ������ �������� 10 ������

        foreach (var collider in colliders)
        {
            if (collider.CompareTag(comparedTag))
            {
                itemCount++;
            }
        }

        return itemCount;
    }
}