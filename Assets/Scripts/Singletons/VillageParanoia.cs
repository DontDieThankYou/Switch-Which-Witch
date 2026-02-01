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
    public float resetTime = 5;
    public float resetTimer = 0;

    void Awake()
    {
        if(instance != null) Destroy(this);
        
        instance = this;
    }
    void FixedUpdate()
    {
        if((!susCaught || !susTied) && resetTimer >= 0)
        {
            resetTimer -= Time.fixedDeltaTime;
            if(resetTimer < 0)
            {
                Reset();
            }
        }
    }
    public void resetTimerTime()
    {
        resetTimer = resetTime;
    }
    public void SetSusCaught(bool val)
    {
        susCaught = val;
        resetTimer = resetTime;
    }
    
    public void SetSusTied(bool val)
    {
        susTied = val;
        resetTimer = resetTime;
    }
    void Reset()
    {
        foreach(GameObject g in villagers)
        {
            EnemyActions v = g.GetComponent<EnemyActions>();
            v.SkipLynch();
        }
    }
    public void AttemptAccuse(EnemyActions villager)
    {
        VillagePyreDestination.Reset();
        if (true || paranoia > accusationThreshold)
        {
            susCaught = false;
            susTied = false;
            Accuse(villager);
            HUDController.INSTANCE.CancelAccuse();
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
}