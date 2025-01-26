using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteractor : MonoBehaviour
{
    public float interactRange = 2f;
    public LayerMask interactableLayer;
    public TMP_Text interactionText;

    private void Awake()
    {
        var inputActions = new @InputSystem_Actions();
        inputActions.Player.Interact.started += ctx => TryInteract();
        inputActions.Enable();
    }

    private void Update()
    {
        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactRange, interactableLayer))
        {
            //IUsable usableObject = hit.collider.GetComponent<IUsable>();
            //if (usableObject != null)
            if (hit.collider.CompareTag("Selectable"))
            {
                interactionText.text = "Press E to interact";
                return;
            }
        }
        interactionText.text = string.Empty;
    }

    private void TryInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactRange, interactableLayer))
        {
            IUsable usableObject = hit.collider.GetComponent<IUsable>();
            if (usableObject != null)
            {
                usableObject.Use();
            }
        }
    }
}
