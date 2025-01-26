using System.Collections;
using UnityEngine;

public class QuestTriggerSceneStart : MonoBehaviour
{
    public QuestManager questManager;  // ������ �� �������� �������
    public int triggerQuestId;         // ID ������, ��� ������� �������� ���-�����
    public float delayBeforeStart = 12f; // �������� ����� ������� ���-�����
    public CutsceneController cutsceneController;
    public GameObject uiPanel;

    private bool isTriggered = false;

    private void Start()
    {
        // �������� �� ������� ���������� ������
        questManager.OnQuestAdded += OnQuestReceived;
    }

    private void OnDestroy()
    {
        // ������� �� ������� ��� ����������� �������
        questManager.OnQuestAdded -= OnQuestReceived;
    }

    private void OnQuestReceived(int questId)
    {
        // ���� ����� � ������ ID ��� ������� � ���-����� ��� �� ��������
        if (questId == triggerQuestId && !isTriggered)
        {
            StartCoroutine(StartCutsceneAfterDelay());
        }
    }

    private IEnumerator StartCutsceneAfterDelay()
    {
        isTriggered = true; // ������������� ����, ����� �� ��������� ���-����� �����
        Debug.Log("Quest received, starting cutscene...");

        // ���� ��������� ���������� ������
        yield return new WaitForSeconds(delayBeforeStart);

        // ����� ����� �������� ������ ��� ������ ���-�����
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