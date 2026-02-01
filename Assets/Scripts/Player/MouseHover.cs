using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MouseHover : MonoBehaviour
{
    Outline objectHighlighted;
    EnemyActions highlightedVillager;
    public LayerMask villagerLayer;
    PlayerInput input;
    public AudioClip[] accuse;
    public AudioSource accuseSource;
    public AudioSource HexSource;

    void Start()
    {
        input = PlayerController.instance.playerInput;
        input.actions["Click"].performed += OnPointerUp;
    }
    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if(Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, villagerLayer)
            && hitInfo.transform.TryGetComponent<Outline>(out Outline outline)
            && hitInfo.transform.parent != null
            && hitInfo.transform.parent.TryGetComponent<EnemyActions>(out EnemyActions actions)
            && !actions.isHexed && !actions.isAccused)
        {
            if(objectHighlighted != null && objectHighlighted != outline)
            {
                objectHighlighted.Deactivate();
            }
            if (objectHighlighted == null || objectHighlighted != outline)
            {
                objectHighlighted = outline;
                objectHighlighted.Activate(Color.purple);
                highlightedVillager = actions;
            }
        }
        else if(objectHighlighted != null)
        {
            if(highlightedVillager != null && !highlightedVillager.isHexed) objectHighlighted.Deactivate();
            objectHighlighted = null;
        }
    }

    public void OnPointerUp(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<float>() <= 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, villagerLayer)
            && hitInfo.transform.TryGetComponent<Outline>(out Outline outline)
            && hitInfo.transform.parent != null
            && hitInfo.transform.parent.TryGetComponent<EnemyActions>(out EnemyActions actions)
            && !actions.isHexed
            && !actions.isAccused)
        {
            if(PlayerController.instance.isAccusing)
            {
                outline.Activate(Color.red);
                VillageParanoia.instance.AttemptAccuse(actions);
                HUDController.INSTANCE.CancelAccuse();

                int index = UnityEngine.Random.Range(0,accuse.Count());
                accuseSource.clip = accuse[index];
                AudioManager.instance.PlayAudioSource(true, accuseSource);
            }
            else
            {
                outline.Activate(Color.blue);
                actions.Hex();
                AudioManager.instance.PlayAudioSource(true, HexSource);
            }
        }
        }
    }
    void OnDestroy()
    {
        input.actions["Click"].performed -= OnPointerUp;
    }
}
