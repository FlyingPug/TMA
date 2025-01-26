using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour, IUsable
{
    public Transform openPosition;
    public float openCloseTime = 2f;
    public bool smoothStart = true;
    public bool smoothEnd = true;
    public AudioClip openSound;
    public AudioClip closeSound;
    public float openVolume = 1f;
    public float closeVolume = 1f;

    public bool isDoubleDoor = false; // Является ли дверь двустворчатой
    public DoorController secondDoor; // Вторая створка (если есть)

    private Vector3 closedPosition;
    private bool isOpen = false; // Текущее состояние двери
    private bool isMoving = false;
    private Coroutine doorCoroutine;
    private AudioSource audioSource;
    private string originalTag;
    

    private void Start()
    {
        closedPosition = transform.position;
        audioSource = gameObject.AddComponent<AudioSource>();
        originalTag = gameObject.tag;
    }

    public void Use()
    {
        if (isMoving) return;

        // Если дверь двустворчатая, синхронизируем состояние со второй створкой
        if (isDoubleDoor && secondDoor != null)
        {
            if (secondDoor.isMoving) return;

            // Открываем или закрываем обе двери в зависимости от текущего состояния
            bool targetState = !isOpen;
            SetState(targetState);
            secondDoor.SetState(targetState);
        }
        else
        {
            // Открываем/закрываем одиночную дверь
            ToggleState();
        }
    }

    private void SetState(bool open)
    {
        if (isOpen == open) return; // Уже в нужном состоянии

        if (doorCoroutine != null)
            StopCoroutine(doorCoroutine);

        Vector3 targetPosition = open ? openPosition.position : closedPosition;
        AudioClip sound = open ? openSound : closeSound;
        float volume = open ? openVolume : closeVolume;

        doorCoroutine = StartCoroutine(MoveDoor(targetPosition, sound, volume));
        isOpen = open;
    }

    private void ToggleState()
    {
        SetState(!isOpen);
    }

    private IEnumerator MoveDoor(Vector3 targetPosition, AudioClip sound, float volume)
    {
        isMoving = true;
        gameObject.tag = "Untagged";

        Vector3 startPos = transform.position;
        float elapsedTime = 0f;

        if (sound != null)
            audioSource.PlayOneShot(sound, volume);

        while (elapsedTime < openCloseTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / openCloseTime;

            if (smoothStart)
                t = Mathf.SmoothStep(0f, 1f, t);
            if (smoothEnd)
                t = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(startPos, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
        gameObject.tag = originalTag;
    }
}
