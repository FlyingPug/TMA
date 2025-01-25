using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Настройки взаимодействия")]
    public float interactRange = 2f;  // Радиус взаимодействия
    public LayerMask interactableLayer;  // Слой с объектами, доступными для взаимодействия

    private void Awake()
    {
        var inputActions = new @InputSystem_Actions();
        inputActions.Player.Interact.started += ctx => TryInteract();
        inputActions.Enable();
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