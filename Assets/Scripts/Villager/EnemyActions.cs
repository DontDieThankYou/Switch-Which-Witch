using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.AI;

public class EnemyActions : MonoBehaviour
{
    // regular pathfinding
    // regions with weights if nothing overrides

    [SerializeField] NavMeshAgent navMeshAgent;
    VillageNavigation villageNavigation;

    bool normalPathfinding = true;
    float waitingTimer = 0.0f;

    void Start()
    {
        villageNavigation = VillageNavigation.instance;
    }
    
    void FixedUpdate()
    {
        if (normalPathfinding)
        {
            Debug.Log("tick");
            // during normal pathfinding
            if (navMeshAgent.hasPath && navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
            {
                // assumes path endpoint is reachable
                // nothing i guess
            } else if (waitingTimer > 0)
            {
                waitingTimer -= Time.fixedDeltaTime;
            } else
            {
                // create new path
                navMeshAgent.destination = villageNavigation.PickRandomWeightedLocation();
            }
        }
    }
}