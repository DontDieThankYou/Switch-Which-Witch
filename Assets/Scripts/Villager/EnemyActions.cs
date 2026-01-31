using UnityEngine;
using UnityEngine.AI;

public class EnemyActions : MonoBehaviour
{
    // regular pathfinding
    // regions with weights if nothing overrides

    [SerializeField] NavMeshAgent navMeshAgent;
    VillageNavigation villageNavigation;

    bool normalPathfinding = true;
    [SerializeField] float waitingTimer = 0.0f;
    NavArea currentNavArea = null;

    void Start()
    {
        villageNavigation = VillageNavigation.instance;
    }
    
    void FixedUpdate()
    {
        if (normalPathfinding)
        {
            // Debug.Log(navMeshAgent.hasPath);
            // during normal pathfinding
            if (navMeshAgent.hasPath && navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
            {
                // assumes path endpoint is reachable
                // nothing i guess
                Debug.Log("pathing");
            } else
            {
                waitingTimer -= Time.fixedDeltaTime;
                if (waitingTimer < 0)
                {
                    if (currentNavArea == null || Random.Range(0.0f, 1.0f) < 0.2)
                    {
                        // 20% chance to move to another area
                        currentNavArea = villageNavigation.PickWeightedNavArea();
                    }

                    // create new path
                    navMeshAgent.destination = villageNavigation.PickLocationFromNavArea(currentNavArea);
                    waitingTimer = Random.Range(3.0f, 5.0f);
                }
            }
        } else
        {
            // abnormal pathfinding (usually overrides)
        }
    }
}