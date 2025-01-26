using UnityEngine;

public class OutlineSelection : MonoBehaviour
{
    private Transform currentHighlight;
    private Transform previousHighlight;
    private RaycastHit raycastHit;

    public float rayDistance = 10f;
    public Color outlineColor = Color.yellow;
    public float outlineWidth = 9.0f;
    public int interactLayer = 7;

    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance);

        Transform closestHighlight = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (hit.transform.gameObject.layer == interactLayer)
            {
                float distance = Vector3.Distance(ray.origin, hit.point);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestHighlight = hit.transform;
                }
            }
        }

        if (closestHighlight != null)
        {
            if (closestHighlight != previousHighlight)
            {
                HandleHighlight(closestHighlight);

                if (previousHighlight != null)
                {
                    DisableOutline(previousHighlight);
                }

                previousHighlight = closestHighlight;
            }
        }
        else
        {
            ClearHighlight();
        }
    }

    private void HandleHighlight(Transform target)
    {
        EnableOutline(target);

        // Проверяем, является ли объект дверью и двустворчатой
        DoorController door = target.GetComponent<DoorController>();
        if (door != null && door.isDoubleDoor && door.secondDoor != null)
        {
            EnableOutline(door.secondDoor.transform);
        }
    }

    private void EnableOutline(Transform target)
    {
        Outline outline = target.GetComponent<Outline>();
        if (outline == null)
        {
            outline = target.gameObject.AddComponent<Outline>();
            outline.OutlineColor = outlineColor;
            outline.OutlineWidth = outlineWidth;
        }
        outline.enabled = true;
    }

    private void DisableOutline(Transform target)
    {
        Outline outline = target.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    private void ClearHighlight()
    {
        if (previousHighlight != null)
        {
            DisableOutline(previousHighlight);

            // Убираем контур второй створки, если есть
            DoorController door = previousHighlight.GetComponent<DoorController>();
            if (door != null && door.isDoubleDoor && door.secondDoor != null)
            {
                DisableOutline(door.secondDoor.transform);
            }

            previousHighlight = null;
        }
    }
}
