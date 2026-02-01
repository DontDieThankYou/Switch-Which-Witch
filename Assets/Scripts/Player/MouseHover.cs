using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MouseHover : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Outline objectHighlighted;
    EnemyActions highlightedVillager;
    public LayerMask villagerLayer;
    private Outline onDownObject;
    private EnemyActions onDownVillager;

    void Awake()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        if(Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, villagerLayer)
            && hitInfo.transform.TryGetComponent<Outline>(out Outline outline)
            && hitInfo.transform.parent != null
            && hitInfo.transform.parent.TryGetComponent<EnemyActions>(out EnemyActions actions)
            && !actions.isHexed)
        {
            if(objectHighlighted != null && objectHighlighted != outline)
            {
                objectHighlighted.Deactivate();
            }
            if (objectHighlighted == null || objectHighlighted != outline)
            {
                objectHighlighted = outline;
                objectHighlighted.Activate(Color.purple);
            }
        }
        else if(objectHighlighted != null)
        {
            if(highlightedVillager != null && !highlightedVillager.isHexed) objectHighlighted.Deactivate();
            objectHighlighted = null;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, villagerLayer)
            && hitInfo.transform.TryGetComponent<Outline>(out Outline outline)
            && hitInfo.transform.parent != null
            && hitInfo.transform.parent.TryGetComponent<EnemyActions>(out EnemyActions actions)
            && !actions.isHexed)
        {
            if(outline == onDownObject && actions == onDownVillager)
            {
                if(PlayerController.instance.isAccusing)
                {
                    objectHighlighted.Activate(Color.red);
                    VillageParanoia.instance.AttemptAccuse(actions);
                }
                else
                {
                    objectHighlighted.Activate(Color.rebeccaPurple);
                    onDownVillager.Hex();
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, villagerLayer)
            && hitInfo.transform.TryGetComponent<Outline>(out Outline outline)
            && hitInfo.transform.parent != null
            && hitInfo.transform.parent.TryGetComponent<EnemyActions>(out EnemyActions actions)
            && !actions.isHexed)
        {
            onDownObject = outline;
            onDownVillager = actions;
        }
    }
}
