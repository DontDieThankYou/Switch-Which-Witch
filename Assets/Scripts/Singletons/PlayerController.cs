using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [Header("Design values")]
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float talismanCraftingTime = 1;
    [SerializeField] float talismanPlacementTime = 1;
    Vector2 moveDir = Vector2.zero;
    [SerializeField] Rigidbody PlayerRoot;
    List<IInteractable> interactables;
    IInteractable currentInteractable;
    [HideInInspector]public bool isShadowed;
    [HideInInspector]public bool isCrafting;
    [HideInInspector]public bool hasTalisman;
    [HideInInspector]public float talismanCraftingTimer;
    [HideInInspector]public float talismanPlacementTimer = 1;

    PlayerInput playerInput;

    void Awake()
    {
        if(instance != null) Destroy(this);
        instance = this;

        if(PlayerRoot == null)
        {
            Debug.LogError("No Player to Control!");
        }
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Move"].performed += MovePerformed;
        playerInput.actions["Move"].canceled += MoveCanceled;
        playerInput.actions["Interact"].started += InteractStarted;
        playerInput.actions["Interact"].canceled += InteractCanceled;
        playerInput.actions["Craft"].started += CraftStarted;
        playerInput.actions["Craft"].canceled += CraftCanceled;
        interactables = new();
    }

    void FixedUpdate()
    {
        if(PlayerRoot == null) return;

        if(!isCrafting)
        {
            Vector2 vel = moveDir * moveSpeed;
            PlayerRoot.linearVelocity = new Vector3(vel.x, 0, vel.y);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IInteractable interactable)) interactables.Add(interactable);
    }
    void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out IInteractable interactable)) interactables.Remove(interactable);
    }
    private void MovePerformed(InputAction.CallbackContext ctx)
    {
        moveDir = ctx.ReadValue<Vector2>().normalized;
    }
    private void MoveCanceled(InputAction.CallbackContext ctx)
    {
        moveDir = Vector2.zero;
    }
    private void InteractStarted(InputAction.CallbackContext ctx)
    {
        bool interact = ctx.ReadValue<bool>();
        if (!interact) return;

        foreach(IInteractable interactable in interactables)
        {
            if(interactable.IsInteractable())
            {
                currentInteractable = interactable;
                interactable.Interact();
                break;
            }
        }
    }
    private void InteractCanceled(InputAction.CallbackContext ctx)
    {
        bool interact = ctx.ReadValue<bool>();
        if (interact) return;

        currentInteractable.CancelInteract();
        currentInteractable = null;
    }
    private void CraftStarted(InputAction.CallbackContext ctx)
    {
        if(isCrafting) return;
        isCrafting = true;
    }
    private void CraftCanceled(InputAction.CallbackContext ctx)
    {
        if(!isCrafting) return;
        isCrafting = false;
    }
    private void Reset()
    {
        isCrafting = false;
        isShadowed = false;
        interactables.Clear();
        moveDir = Vector2.zero;
    }
    private void OnDestroy()
    {
        playerInput.actions["Move"].performed -= MovePerformed;
        playerInput.actions["Move"].canceled -= MoveCanceled;
        playerInput.actions["Interact"].started += InteractStarted;
        playerInput.actions["Craft"].started += CraftStarted;
        playerInput.actions["Craft"].canceled += CraftCanceled;
    }
}
