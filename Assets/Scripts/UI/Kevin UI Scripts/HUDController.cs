using System.Collections;
using DG.Tweening;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public static HUDController INSTANCE;
    private CanvasGroup cg;
    [SerializeField] GameObject globalVolume;
    [SerializeField] GameObject accuseVolume;

    private PlayerController playerController;

    private bool accused = false;
    public AudioSource audioSource;
    public AudioSource loopSource;
    public AudioClip AccuseOff;
    public AudioClip AccuseOn;

    [Header("Accuse Shit")]
    private bool canClick = true;
    [SerializeField] private CanvasGroup colouredAccuse;

    void Awake()
    {
        INSTANCE = this;
        cg = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        playerController = PlayerController.instance;
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
            audioSource.clip = AccuseOn;
            AudioManager.instance.PlayAudioSource(true, audioSource);
            
            if(!loopSource.isPlaying)
            {
                AudioManager.instance.PlayAudioSource(false, loopSource);
            }

            // turn on accuse
            // call accuse manager or some shit to turn on the global volume or whatever
            colouredAccuse.DOFade(1f, 1f).OnComplete(() =>
            {
                canClick = true;
            });

            if (playerController.isCrafting)
            {
                playerController.CraftCanceled();
            }
            playerController.InteractCanceled();

            playerController.isAccusing = true;
            accuseVolume.SetActive(true);
            globalVolume.SetActive(false);

        } else
        {
            audioSource.clip = AccuseOff;
            AudioManager.instance.PlayAudioSource(true, audioSource);
            muffleAmbience();
            if(!loopSource.isPlaying)
            {
                loopSource.Stop();
            }
            // turn off accuse 
            // do some bullshit with accuse manager again

            colouredAccuse.DOFade(0f,1f).OnComplete(() =>
            {
               canClick = true; 
            });

            globalVolume.SetActive(true);
            accuseVolume.SetActive(false);
        }

        accused = !accused;
    }

    // from Kevin -call this when you perform an accuse action to automatically do the 
    public void CancelAccuse()
    {
        if (playerController.isAccusing) return;

        canClick = false;
        colouredAccuse.DOFade(0f,1f).OnComplete(() =>
        {
            canClick = true; 
        });

        playerController.isAccusing = false;
        globalVolume.SetActive(true);
        accuseVolume.SetActive(false);
    }
    IEnumerator muffleAmbience()
    {
        AudioManager.instance.ambienceSource.volume *= 0.8f;
        yield return new WaitForSeconds(1);
        AudioManager.instance.ambienceSource.volume /= 0.8f;
    }
    
}
