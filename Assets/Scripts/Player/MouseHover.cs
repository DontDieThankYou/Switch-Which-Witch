using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MouseHover : MonoBehaviour
{
    Outline objectHighlighted;

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        if(Physics.Raycast(ray, out RaycastHit hitInfo)
            && hitInfo.transform.TryGetComponent<Outline>(out Outline outline)) //TODO Change when can check for villager
        {
            if(objectHighlighted != null && objectHighlighted != outline)
            {
                objectHighlighted.Deactivate();
            }
            if (objectHighlighted == null || objectHighlighted != outline)
            {
                objectHighlighted = outline;
                objectHighlighted.Activate();
            }
        }
        else if(objectHighlighted != null)
        {
            objectHighlighted.Deactivate();
            objectHighlighted = null;
        }
    }
}
