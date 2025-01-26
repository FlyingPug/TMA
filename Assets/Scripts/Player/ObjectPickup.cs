using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectPickupAction : MonoBehaviour
{
    public float pickupRange = 3f;
    public float moveForce = 10f;
    public Transform holdPosition;

    private GameObject heldObject;
    private Rigidbody heldObjectRb;
    private @InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new @InputSystem_Actions();
        inputActions.Player.Interact.started += _ => HandlePickup();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void HandlePickup()
    {
        Console.WriteLine("picking up");
        if (heldObject == null)
        {
            TryPickupObject();
        }
        else
        {
            DropObject();
        }
    }

    void TryPickupObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Pickup"))            
            {
                heldObject = hit.collider.gameObject;
                heldObjectRb = heldObject.GetComponent<Rigidbody>();
                heldObjectRb.useGravity = false;
                heldObjectRb.linearDamping = 10f;
            }
        }
    }

    void MoveObject()
    {
        if (heldObject != null)
        {
            Vector3 direction = holdPosition.position - heldObject.transform.position;
            heldObjectRb.linearVelocity = direction * moveForce;
        }
    }

    void DropObject()
    {
        heldObjectRb.useGravity = true;
        heldObjectRb.linearDamping = 1f;
        heldObject = null;
    }

    private void Update()
    {
        if (heldObject != null)
        {
            MoveObject();
        }
    }
}