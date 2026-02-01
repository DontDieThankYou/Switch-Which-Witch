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
    [HideInInspector] public bool hasTalisman = true;
    [HideInInspector] public float talismanCraftingTimer;
    [HideInInspector]public float talismanPlacementTimer;
    [SerializeField]private Animator animController;

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
        if (!(moveDir.y != 0 && moveDir.x != 0)) animController.SetBool("isMoving", true); 
        
        if (moveDir.x < 0) // Left
        {
            animController.SetInteger("direction", 3);
        }
        else if (moveDir.x > 0) // Right
        {
            animController.SetInteger("direction", 4);
        }
        else if (moveDir.y < 0) // Down
        {
            animController.SetInteger("direction", 1);
        }
        else if (moveDir.y > 0) // Up
        {
            animController.SetInteger("direction", 2);
        }
    }
    private void MoveCanceled(InputAction.CallbackContext ctx)
    {
        moveDir = Vector2.zero;
        animController.SetBool("isMoving", false);
        animController.SetInteger("direction", 0);
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
