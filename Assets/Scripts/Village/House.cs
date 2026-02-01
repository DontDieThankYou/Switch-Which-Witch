using UnityEngine;

public class House : MonoBehaviour, IInteractable
{
    public bool hasTalisman;
    [SerializeField] public SpriteRenderer talisman;
    void Awake()
    {
        talisman.enabled = hasTalisman;
    }

    public InteractableType Interact()
    {
        return InteractableType.House;
    }

    public bool IsInteractable()
    {
        return PlayerController.instance.hasTalisman && !hasTalisman;
    }
    public void RemoveTalisman()
    {
        hasTalisman = false;
        talisman.enabled = false;
    }
    public void PlaceTalisman()
    {
        PlayerController.instance.hasTalisman = false;
        hasTalisman = true;
        talisman.enabled = true;
    }

    public void CancelInteract(){}
    public bool TryGetGameObject(out GameObject g)
    {
        g = this.gameObject;
        return true;
    }
}
