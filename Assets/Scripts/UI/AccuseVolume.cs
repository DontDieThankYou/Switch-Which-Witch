using UnityEngine;
using UnityEngine.Rendering;

public class AccuseVolume : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float minAbberation;
    [SerializeField] float maxAbberation;
    [SerializeField] float minVignette;
    [SerializeField] float maxVignette;
    [SerializeField] float phaseTime;
    [SerializeField] VolumeProfile volume;
    float time = 0.0f;
    int side = 1;
    UnityEngine.Rendering.Universal.ChromaticAberration chromaticAberration;
    UnityEngine.Rendering.Universal.Vignette vignette;
    void Start()
    {
        volume.TryGet(out chromaticAberration);
        volume.TryGet(out vignette);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * side;
        if (time > phaseTime)
        {
            time = phaseTime;
            side = -1;
        } else if (time < 0)
        {
            time = 0.0f;
            side = 1;
        }

        chromaticAberration.intensity.Override(Mathf.Lerp(minAbberation, maxAbberation, time / phaseTime));
        vignette.intensity.Override(Mathf.Lerp(minVignette, maxVignette, time / phaseTime));
    }
}
