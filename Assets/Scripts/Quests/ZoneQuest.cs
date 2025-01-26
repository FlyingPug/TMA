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
    public int requiredItemsCount = 1;  // Количество предметов, которое должно быть в зоне


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

        // Проверка, что в зоне есть нужное количество предметов
        int currentItemsCount = CountItemsInZone();
        if (currentItemsCount < requiredItemsCount)
        {
            Debug.Log($"Not enough items in the zone. Required: {requiredItemsCount}, Found: {currentItemsCount}");
            yield break; // Прерываем выполнение квеста, если недостаточно предметов
        }

        // Показываем субтитры
        foreach (int subtitleId in subtitleIds)
        {
            questManager.subtitleManager.EnqueueSubtitle(subtitleId);
            yield return new WaitForSeconds(0.0f); // Можно заменить на ожидание окончания субтитров, если необходимо
        }

        // Завершаем текущий квест и добавляем следующий
        questManager.CompleteQuest(completedQuestId);
        questManager.AddQuest(nextQuestId);
    }

    // Метод для подсчета количества предметов в зоне (простейший пример)
    private int CountItemsInZone()
    {
        int itemCount = 0;
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10f); // Радиус проверки 10 единиц

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