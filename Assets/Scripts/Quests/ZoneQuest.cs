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
    public int requiredItemsCount = 1;  // ��������� ���������� ��������� � ����

    private HashSet<GameObject> itemsInZone = new HashSet<GameObject>();  // �������� � ����

    private void Start()
    {
        // �������� �� ������� ����� ������ � questManager
        questManager.OnQuestAdded += OnQuestAdded;
    }

    private void OnDestroy()
    {
        // ������� �� �������, ���� ������ ���������
        questManager.OnQuestAdded -= OnQuestAdded;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ��������� ������� � ���� ������ ���� ��� ������� � ������ �����
        if (other.CompareTag(comparedTag))
        {
            itemsInZone.Add(other.gameObject);
            StartCoroutine(HandleQuestStart());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ������� ������� �� ������ ��� ������ �� ����
        if (other.CompareTag(comparedTag))
        {
            itemsInZone.Remove(other.gameObject);
        }
    }

    // ����� ��� �������� �� ������� ����� ������
    private void OnQuestAdded(int questId)
    {
        // ����� ����� ��������, ���������, ����� �� ��������� ����� ��� ���� ����
        if (questId == completedQuestId)
        {
            StartCoroutine(HandleQuestStart());
        }
    }

    private IEnumerator HandleQuestStart()
    {
        // ������� ���������� ���������, ���� ��� � ��������
        yield return new WaitUntil(() => questManager.subtitleManager.QueueLength == 0);

        // �������� ������� ������ ������
        if (itemsInZone.Count >= requiredItemsCount)
        {
            Quest completedQuest = questManager.currentQuests.Find(q => q.id == completedQuestId);

            if (completedQuest != null && !completedQuest.isCompleted)
            {
                // ���������� ��������
                foreach (int subtitleId in subtitleIds)
                {
                    questManager.subtitleManager.EnqueueSubtitle(subtitleId);
                    yield return null; // ���� ���������� ��������� (���� �����, ����� ������������ WaitForSeconds)
                }

                // ��������� ������� ����� � �������� ���������
                questManager.CompleteQuest(completedQuestId);
                questManager.AddQuest(nextQuestId);
            }
        }
    }
}