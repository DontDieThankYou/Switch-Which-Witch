using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable] public class ActionPoints
{
    public float paranoia;
    public float suspicion;
}

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
    [HideInInspector]public bool isAccusing;
    [HideInInspector]public float talismanPlacementTimer;
    [SerializeField]private Animator animController;
    [HideInInspector] public float suspicion = 0.0f;
    [Header("Points")]
    [SerializeField] public ActionPoints nightmarePoints;
    [SerializeField] public ActionPoints talismanPoints;
    [SerializeField] public ActionPoints hexPoints;
    VillageParanoia villageParanoia;


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
        playerInput.actions["Interact"].canceled += InputInteractCanceled;
        playerInput.actions["Craft"].started += CraftStarted;
        playerInput.actions["Craft"].canceled += InputCraftCanceled;
        interactables = new();
        villageParanoia = VillageParanoia.instance;
    }

    void FixedUpdate()
    {
        if(PlayerRoot == null) return;

        if (!isAccusing)
        {

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
                    // add points for talisman
                    suspicion += talismanPoints.suspicion;
                    villageParanoia.paranoia += talismanPoints.paranoia;
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
        if (interact < 0)
        {
            return;
        } else if (isPlacingTalisman || isCrafting) {
            return;
        }

        foreach(IInteractable interactable in interactables)
        {
            if(interactable.IsInteractable())
            {
                currentInteractable = interactable;
                InteractableType t = interactable.Interact();
                switch (t)
                {
                    case InteractableType.House:
                        isPlacingTalisman = true;
                        talismanPlacementTimer = talismanPlacementTime;
                        break;
                }
                break;
            }
        }
    }

    private void timeInteractClick()
    {
        
    }
    
    private void InputInteractCanceled(InputAction.CallbackContext ctx)
    {
        float interact = ctx.ReadValue<float>();
        if (interact > 0) return;
        InteractCanceled();
    }

    public void InteractCanceled()
    {
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
        TalismanHUDController.INSTANCE.IsFilling(true);
    }
    private void InputCraftCanceled(InputAction.CallbackContext ctx)
    {
        float craft = ctx.ReadValue<float>();
        if(craft > 0 || !isCrafting) return;
        CraftCanceled();
    }
    public void CraftCanceled()
    {
        isCrafting = false;
        TalismanHUDController.INSTANCE.IsFilling(false);
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
        playerInput.actions["Interact"].started -= InteractStarted;
        playerInput.actions["Interact"].canceled -= InputInteractCanceled;
        playerInput.actions["Craft"].started -= CraftStarted;
        playerInput.actions["Craft"].canceled -= InputCraftCanceled;
    }

    public float TalismanPlacementTime => talismanCraftingTime;
}
