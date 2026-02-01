using UnityEngine;

public class House : MonoBehaviour, IInteractable
{
    public bool hasTalisman;
    public bool isLightsOn = true;
    [SerializeField] public SpriteRenderer talisman;
    [SerializeField] public Light talismanHalo;
    void Awake()
    {
        talisman.enabled = hasTalisman;
        talismanHalo.enabled = hasTalisman;
    }

    public InteractableType Interact()
    {
        return InteractableType.House;
    }

    public bool IsInteractable()
    {
        return (PlayerController.instance.hasTalisman && !hasTalisman) || !isLightsOn;
    }
    public void RemoveTalisman()
    {
        hasTalisman = false;
        talisman.enabled = false;
        talismanHalo.enabled = (false);
    }
    public void PlaceTalisman()
    {
        PlayerController.instance.hasTalisman = false;
        hasTalisman = true;
        talisman.enabled = true;
        talismanHalo.enabled = (true);
    }

    public void CancelInteract(){}
    public bool TryGetGameObject(out GameObject g)
    {
        g = this.gameObject;
        return true;
    }

    public InteractableType GetInteractableType()
    {
        return InteractableType.House;
    }
}
