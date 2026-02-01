using System.Linq;
using UnityEngine;

public class Pyre : MonoBehaviour
{
    public static Pyre instance;
    public SpriteRenderer villagerSP;
    public Sprite[] villagerVariants;
    public AudioClip[] burns;
    public AudioSource audioSource;
    public ParticleSystem ps;
    bool isBurning;
    
    void Awake()
    {
        if(instance != null) Destroy(this);
        instance = this;
        villagerSP.enabled = false;
    }
    public void TiePyre(int villVar)
    {
        isBurning = true;
        villagerSP.sprite = villagerVariants[villVar];
        villagerSP.enabled = true;
        VillageParanoia.instance.SetSusTied(true);
    }
    public void LightPyre()
    {
        ps.Play();
        int index = Random.Range(0, burns.Count());
        audioSource.clip = burns[index];
        AudioManager.instance.PlayAudioSource(true, audioSource);
    }
    public void DismissPyre()
    {
        if (!isBurning) return;
        ps.Stop();
        isBurning = false;
        villagerSP.enabled = false;
    }
}
