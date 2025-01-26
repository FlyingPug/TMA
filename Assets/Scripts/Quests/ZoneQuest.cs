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
    public int requiredItemsCount = 1;  // Требуемое количество предметов в зоне

    private HashSet<GameObject> itemsInZone = new HashSet<GameObject>();  // Предметы в зоне

    private void Start()
    {
        // Подписка на событие смены квеста в questManager
        questManager.OnQuestAdded += OnQuestAdded;
    }

    private void OnDestroy()
    {
        // Отписка от события, если объект уничтожен
        questManager.OnQuestAdded -= OnQuestAdded;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Добавляем предмет в зону только если это предмет с нужным тегом
        if (other.CompareTag(comparedTag))
        {
            itemsInZone.Add(other.gameObject);
            StartCoroutine(HandleQuestStart());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Убираем предмет из списка при выходе из зоны
        if (other.CompareTag(comparedTag))
        {
            itemsInZone.Remove(other.gameObject);
        }
    }

    // Метод для подписки на событие смены квеста
    private void OnQuestAdded(int questId)
    {
        // Когда квест добавлен, проверяем, нужно ли запускать квест для этой зоны
        if (questId == completedQuestId)
        {
            StartCoroutine(HandleQuestStart());
        }
    }

    private IEnumerator HandleQuestStart()
    {
        // Ожидаем завершения субтитров, если они в процессе
        yield return new WaitUntil(() => questManager.subtitleManager.QueueLength == 0);

        // Проверка условий начала квеста
        if (itemsInZone.Count >= requiredItemsCount)
        {
            Quest completedQuest = questManager.currentQuests.Find(q => q.id == completedQuestId);

            if (completedQuest != null && !completedQuest.isCompleted)
            {
                // Показываем субтитры
                foreach (int subtitleId in subtitleIds)
                {
                    questManager.subtitleManager.EnqueueSubtitle(subtitleId);
                    yield return null; // Ждем завершения субтитров (если нужно, можно использовать WaitForSeconds)
                }

                // Завершаем текущий квест и начинаем следующий
                questManager.CompleteQuest(completedQuestId);
                questManager.AddQuest(nextQuestId);
            }
        }
    }
}