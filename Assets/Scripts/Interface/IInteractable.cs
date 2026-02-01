using UnityEngine;

public enum InteractableType { House, Talisman, Church}
public interface IInteractable
{
    public bool IsInteractable();
    public InteractableType Interact();
    public void CancelInteract();
    public bool TryGetGameObject(out GameObject gameObject);
}
