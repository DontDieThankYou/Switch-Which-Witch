using System;
using System.Collections.Generic;
using UnityEngine;

public class VillageParanoia : MonoBehaviour
{
    public static VillageParanoia instance;
    public static bool susCaught = false;
    public static bool susTied = false;
    public static List<GameObject> villagers = new();

    public float paranoia = 0;
    public readonly float accusationThreshold = 10;

    void Awake()
    {
        if(instance != null) Destroy(this);
        
        instance = this;
    }

    public void AttemptAccuse(EnemyActions villager)
    {
        VillagePyreDestination.Reset();
        if (paranoia > accusationThreshold)
        {
            susCaught = false;
            susTied = false;
            Accuse(villager);
            return;
        }
        AccusePlayer();
    }

    public void Accuse(EnemyActions villager)
    {
        villager.BeAccused();
        foreach(GameObject g in villagers)
        {
            if(g.TryGetComponent<EnemyActions>(out EnemyActions v))
            {
                v.LynchAt(villager.transform.position);
            }
        }
    }

    public void AccusePlayer()
    {
        PlayerController.instance.IncrementSus(PlayerController.instance.suspicionThreshold * 0.5f);
    }

    public void LynchPlayer()
    {
        susCaught = false;
        susTied = false;
        foreach(GameObject g in villagers)
        {
            if(g.TryGetComponent<EnemyActions>(out EnemyActions v))
            {
                v.LynchAt(PlayerController.instance.transform.position);
            }
        }
    }
    public void WalkToStake()
    {
        
    }
}