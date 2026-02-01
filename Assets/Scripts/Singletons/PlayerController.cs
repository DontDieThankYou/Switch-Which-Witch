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
    [HideInInspector]public bool isPlacingTalisman;
    public bool hasTalisman = true;
    public float talismanCraftingTimer;
    [HideInInspector]public float talismanPlacementTimer;

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

        if(!isCrafting && !isPlacingTalisman)
        {
            Vector2 vel = moveDir * moveSpeed;
            PlayerRoot.linearVelocity = new Vector3(vel.x, 0, vel.y);
        }
        if(isPlacingTalisman && hasTalisman)
        {
            talismanPlacementTimer -= Time.fixedDeltaTime;
            if(talismanPlacementTimer <= 0 && currentInteractable.TryGetGameObject(out GameObject g))
            {
                House h = g.GetComponent<House>();
                h.PlaceTalisman();
            }
        }
        else if(isCrafting && !hasTalisman)
        {
            talismanCraftingTimer -= Time.fixedDeltaTime;
            if(talismanCraftingTimer <= 0)
            {
                hasTalisman = true;
                isCrafting = false;
            }
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
        float interact = ctx.ReadValue<float>();
        if (interact < 0 || isPlacingTalisman || isCrafting) return;

        foreach(IInteractable interactable in interactables)
        {
            if(interactable.IsInteractable())
            {
                currentInteractable = interactable;
                InteractableType t = interactable.Interact();
                if(t == InteractableType.House)
                {
                    isPlacingTalisman = true;
                    talismanPlacementTimer = talismanPlacementTime;
                }
                break;
            }
        }
    }
    private void InteractCanceled(InputAction.CallbackContext ctx)
    {
        float interact = ctx.ReadValue<float>();
        if (interact > 0) return;
        
        if(isPlacingTalisman)
        {
            hasTalisman = false;
            isPlacingTalisman = false;
        }

        currentInteractable?.CancelInteract();
        currentInteractable = null;
    }
    private void CraftStarted(InputAction.CallbackContext ctx)
    {
        float craft = ctx.ReadValue<float>();
        if(craft < 0 || isCrafting || hasTalisman || isPlacingTalisman) return;
        isCrafting = true;
        talismanCraftingTimer = talismanCraftingTime;
    }
    private void CraftCanceled(InputAction.CallbackContext ctx)
    {
        float craft = ctx.ReadValue<float>();
        if(craft > 0 || !isCrafting) return;
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
