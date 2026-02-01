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
            if(!highlightedVillager.isHexed) objectHighlighted.Deactivate();
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
                objectHighlighted.Activate(Color.rebeccaPurple);
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
