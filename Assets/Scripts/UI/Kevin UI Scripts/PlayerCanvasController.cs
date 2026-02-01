using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject indicator;
    private bool indicatorVisible = false;
    private Tween indicatorTween;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
    }

    void Start()
    {
        PlayerController.instance.enterInteractable += EnterInteractable;
        PlayerController.instance.exitInteractable += ExitInteractable;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.interactables.Count != 0 && 
            ((!PlayerController.instance.hasTalisman && indicatorVisible) ||
            PlayerController.instance.hasTalisman && !indicatorVisible && PlayerController.instance.interactables[0].IsInteractable()))
        {
            ToggleInteract(!indicatorVisible);
        }
    }

    private void EnterInteractable(IInteractable interactable)
    {
        if (PlayerController.instance.interactables[0].IsInteractable())
            ToggleInteract(true);
    }

    private void ExitInteractable(IInteractable interactable)
    {
        if (PlayerController.instance.interactables.Count == 0)
        {
            ToggleInteract(false);
        }
    }

    private void ToggleInteract(bool toggleOn)
    {
        indicatorTween?.Kill();

        if (toggleOn)
        {
            indicatorVisible = true;
            indicatorTween = indicator.GetComponent<CanvasGroup>()
                .DOFade(1f, 0.7f)
                .From(0f)
                .SetEase(Ease.OutBack);

        } else
        {
            indicatorVisible = false;
            indicatorTween = indicator.GetComponent<CanvasGroup>()
                .DOFade(0f, 0.25f)
                .SetEase(Ease.InBack);
        }
    }
}
