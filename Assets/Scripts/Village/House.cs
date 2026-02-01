using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour, IInteractable
{
    public bool hasTalisman;
    public bool isLightsOn = true;
    [SerializeField] public SpriteRenderer talisman;
    public float talismanTime = 10;
    public float talismanTimer = 0;
    [SerializeField] public Light talismanHalo;
    public ParticleSystem burstPs;

    public static List<Vector3> doors = new();
    void Awake()
    {
        talisman.enabled = hasTalisman;
        talismanHalo.enabled = hasTalisman;
        doors.Add(transform.position);
    }
    void FixedUpdate()
    {
        if(hasTalisman)
        {
            talismanTimer -= Time.deltaTime;
            if(talismanTimer <= 0)
            {
                RemoveTalisman();
            }
        }
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
        talismanHalo.enabled = false;
    }
    public void PlaceTalisman()
    {
        talismanTimer = talismanTime;
        PlayerController.instance.hasTalisman = false;
        hasTalisman = true;
        talisman.enabled = true;
        talismanHalo.enabled = true;
        burstPs.Play();
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
