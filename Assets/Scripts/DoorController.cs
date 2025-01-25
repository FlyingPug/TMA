using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour, IUsable
{
    [Header("Настройки двери")]
    public Transform openPosition;
    public float openCloseTime = 2f;
    public bool smoothStart = true;
    public bool smoothEnd = true; 

    private Vector3 closedPosition;
    private bool isOpen = false;
    private Coroutine doorCoroutine;

    private void Start()
    {
        closedPosition = transform.position;
    }

    public void Use()
    {
        if (doorCoroutine != null)
            StopCoroutine(doorCoroutine);

        doorCoroutine = StartCoroutine(MoveDoor(isOpen ? closedPosition : openPosition.position));
        isOpen = !isOpen;
    }

    private IEnumerator MoveDoor(Vector3 targetPosition)
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;

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
    }
}
