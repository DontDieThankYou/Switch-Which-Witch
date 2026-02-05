using UnityEngine;
using UnityEngine.Rendering;

public class NormalVolume : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float minVignette;
    [SerializeField] float maxVignette;
    [SerializeField] float minSaturation;
    [SerializeField] float maxSaturation;
    [SerializeField] VolumeProfile volume;
    UnityEngine.Rendering.Universal.Vignette vignette;
    UnityEngine.Rendering.Universal.ColorAdjustments colorAdjustments;
    float displayedParanoia;
    [SerializeField] float paraniaChangePerSecond = 1.0f;
    void Start()
    {
        volume.TryGet(out vignette);
        volume.TryGet(out colorAdjustments);
    }

    // Update is called once per frame
    void Update()
    {
        vignette.intensity.Override(Mathf.Lerp(minVignette, maxVignette, displayedParanoia / 100.0f));
        colorAdjustments.saturation.Override(Mathf.Lerp(minSaturation, maxSaturation, displayedParanoia / 100.0f));
    }

    void FixedUpdate()
    {
        float paranoia = VillageParanoia.instance.paranoia;
        if (paranoia > displayedParanoia)
        {
            displayedParanoia = Mathf.Min(paranoia, displayedParanoia + paranoia * paraniaChangePerSecond * Time.deltaTime);
        } else if (paranoia < displayedParanoia)
        {
            displayedParanoia = Mathf.Max(paranoia, displayedParanoia - paranoia * paraniaChangePerSecond * Time.deltaTime);
        } else
        {
            return;
        }
    }
}
