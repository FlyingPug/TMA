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

    private Vector3 closedPosition;
    private bool isOpen = false;
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

        if (doorCoroutine != null)
            StopCoroutine(doorCoroutine);

        doorCoroutine = StartCoroutine(MoveDoor(isOpen ? closedPosition : openPosition.position, isOpen ? closeSound : openSound));
        isOpen = !isOpen;
    }

    private IEnumerator MoveDoor(Vector3 targetPosition, AudioClip sound)
    {
        isMoving = true;

        gameObject.tag = "Untagged";

        Vector3 startPos = transform.position;
        float elapsedTime = 0f;

        if (sound != null)
            audioSource.PlayOneShot(sound);

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
