using UnityEngine;

public class Pyre : MonoBehaviour
{
    public static Pyre instance;
    public SpriteRenderer villagerSP;
    public Sprite[] villagerVariants;
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
        VillageParanoia.susTied = true;
    }
    public void LightPyre(){}
    public void DismissPyre()
    {
        if (!isBurning) return;
        isBurning = false;
        villagerSP.enabled = false;
    }
}
