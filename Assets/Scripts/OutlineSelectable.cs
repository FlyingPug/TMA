using UnityEngine;

public class OutlineSelection : MonoBehaviour
{
    private Transform currentHighlight;
    private Transform previousHighlight;
    private RaycastHit raycastHit;

    public float rayDistance = 10f;

    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out raycastHit, rayDistance))
        {
            currentHighlight = raycastHit.transform;

            if (currentHighlight.CompareTag("Selectable"))
            {
                if (currentHighlight != previousHighlight)
                {
                    EnableOutline(currentHighlight);

                    if (previousHighlight != null)
                    {
                        DisableOutline(previousHighlight);
                    }

                    previousHighlight = currentHighlight;
                }
            }
            else
            {
                ClearHighlight();
            }
        }
        else
        {
            ClearHighlight();
        }
    }

    private void EnableOutline(Transform target)
    {
        Outline outline = target.GetComponent<Outline>();
        if (outline == null)
        {
            outline = target.gameObject.AddComponent<Outline>();
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 9.0f;
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
            previousHighlight = null;
        }
    }
}
