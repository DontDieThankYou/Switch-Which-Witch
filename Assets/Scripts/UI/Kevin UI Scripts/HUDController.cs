using DG.Tweening;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public static HUDController INSTANCE;
    private CanvasGroup cg;

    private bool accused = false;

    [Header("Accuse Shit")]
    private bool canClick = true;
    [SerializeField] private CanvasGroup colouredAccuse;

    void Awake()
    {
        INSTANCE = this;
        cg = GetComponent<CanvasGroup>();
    }

    public void ToggleHUD(bool toggleOn)
    {
        cg.DOFade(toggleOn ? 1f : 0f, 0.7f).OnComplete(
            () => cg.interactable = true
        );
    }

    public void ToggleAccuse()
    {
        if (!canClick) return;
        canClick = false;

        if (!accused)
        {
            // turn on accuse
            // call accuse manager or some shit to turn on the global volume or whatever
            colouredAccuse.DOFade(1f, 1f).OnComplete(() =>
            {
                canClick = true;
            });

        } else
        {
            // turn off accuse 
            // do some bullshit with accuse manager again

            colouredAccuse.DOFade(0f,1f).OnComplete(() =>
            {
               canClick = true; 
            });
        }

        accused = !accused;
    }

    // from Kevin -call this when you perform an accuse action to automatically do the 
    public void CancelAccuse()
    {
        if (!accused) return;

        canClick = false;
        colouredAccuse.DOFade(0f,1f).OnComplete(() =>
        {
            canClick = true; 
        });
    }

    
}
