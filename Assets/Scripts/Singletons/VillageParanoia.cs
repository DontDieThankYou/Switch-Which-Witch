using System.Collections.Generic;
using UnityEngine;

public class VillageParanoia : MonoBehaviour
{
    public static VillageParanoia instance;
    public static List<GameObject> villagers = new();

    public float paranoia = 0;
    public readonly float accusationThreshold = 10;

    void Awake()
    {
        if(instance != null) Destroy(this);
        
        instance = this;
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