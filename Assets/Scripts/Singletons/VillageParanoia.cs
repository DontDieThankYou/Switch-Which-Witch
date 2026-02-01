using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;

public class VillageParanoia : MonoBehaviour
{
    public static VillageParanoia instance;
    public static List<GameObject> villagers = new();

    [SerializeField] VolumeProfile volume;
    UnityEngine.Rendering.Universal.Vignette vignette;
    private float targetVignette;
    private float vignetteDeltaPerSecond = 0.2f;
    private float paranoia = 0.0f;
    public float Paranoia
    {
        get {
            return paranoia;
        }
        set {
            paranoia = value;
            targetVignette = Mathf.Lerp(0.23f, 0.8f, paranoia/100.0f);
        }
    }
    public readonly float accusationThreshold = 10;

    void Awake()
    {
        if(instance != null) Destroy(this);
        
        instance = this;

        volume.TryGet(out vignette);
        targetVignette = vignette.intensity.value;
    }

    void Update()
    {
        float currentVignette = vignette.intensity.value;
        if (targetVignette > currentVignette)
        {
            // increase vignette
            currentVignette = Mathf.Min(targetVignette, currentVignette + vignetteDeltaPerSecond * Time.deltaTime);
        } else
        {
            // decrease vignette
            currentVignette = Mathf.Max(targetVignette, currentVignette - vignetteDeltaPerSecond * Time.deltaTime);
        }
        vignette.intensity.Override(currentVignette);
    }

    public void AttemptAccuse()
    {
        VillagePyreDestination.Reset();
        if (paranoia > accusationThreshold)
        {
            Accuse();
        }
        AccusePlayer();
    }

    public void Accuse()
    {
        Debug.LogError("not finished lol");
    }

    public void AccusePlayer()
    {
        
    }

    public void WalkToStake()
    {
        
    }
}