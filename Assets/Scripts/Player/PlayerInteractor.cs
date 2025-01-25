using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [Header("��������� ��������������")]
    public float interactRange = 2f;  // ������ ��������������
    public LayerMask interactableLayer;  // ���� � ���������, ���������� ��� ��������������

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