using System.Collections;
using UnityEngine;

public class QuestTriggerSceneStart : MonoBehaviour
{
    public QuestManager questManager;  // Ссылка на менеджер квестов
    public int triggerQuestId;         // ID квеста, при котором начнется кат-сцена
    public float delayBeforeStart = 12f; // Задержка перед началом кат-сцены
    public CutsceneController cutsceneController;
    public GameObject uiPanel;

    private bool isTriggered = false;

    private void Start()
    {
        // Подписка на событие добавления квеста
        questManager.OnQuestAdded += OnQuestReceived;
    }

    private void OnDestroy()
    {
        // Отписка от события при уничтожении объекта
        questManager.OnQuestAdded -= OnQuestReceived;
    }

    private void OnQuestReceived(int questId)
    {
        // Если квест с нужным ID был получен и кат-сцена еще не началась
        if (questId == triggerQuestId && !isTriggered)
        {
            StartCoroutine(StartCutsceneAfterDelay());
        }
    }

    private IEnumerator StartCutsceneAfterDelay()
    {
        isTriggered = true; // Устанавливаем флаг, чтобы не запускать кат-сцену снова
        Debug.Log("Quest received, starting cutscene...");

        // Ждем указанное количество секунд
        yield return new WaitForSeconds(delayBeforeStart);

        // Здесь можно добавить логику для начала кат-сцены
        StartCutscene();
    }

    private void StartCutscene()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
        }

        cutsceneController.StartCutscene();
    }
}