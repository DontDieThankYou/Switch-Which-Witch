using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class TalismanHUDController : MonoBehaviour
{
    [SerializeField] private Sprite noSprite;
    [SerializeField] private Sprite yesSprite;

    private Slider slider;

    public static TalismanHUDController INSTANCE;
    private Image talismanIndicator;
    private Animation popAnim;

    [Header("State")]

    [SerializeField] private bool isFilled = false;
    [SerializeField] private bool isFilling;
    [SerializeField] private float timer = 0f;

    void Awake()
    {
        INSTANCE = this;
        talismanIndicator = GetComponent<Image>();
        slider = GetComponent<Slider>();
        popAnim = GetComponent<Animation>();
    }

    public void IsFilling(bool toggleOn)
    {
        Debug.Log($"{toggleOn} whaaattttt");
        if (toggleOn)
        {
            isFilling = true;

        } else
        {
            isFilling = false;
            slider.value = 0f;
            timer = 0f;
        }
    }

    void Update()
    {
        if (isFilled) return;

        if (isFilling)
        {
            timer += Time.deltaTime;
            float fill = timer/PlayerController.instance.TalismanPlacementTime;
            slider.value = timer/PlayerController.instance.TalismanPlacementTime;

            if (fill >= 1f)
            {
                Complete();
            }
        }
    }

    private void Complete()
    {
        isFilled = true;
        // AudioManager.instance.PlayAudioSource();
        slider.value = 1f;
        popAnim.Play();   
    }

    public void Interact()
    {
        Debug.Log("Interact!");
        IsFilling(false);
        isFilled = false;
        slider.value = 0f;
    }


}
