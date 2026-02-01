using UnityEngine;
using UnityEngine.AI;

public class EnemyActions : MonoBehaviour
{
    // regular pathfinding
    // regions with weights if nothing overrides

    [SerializeField] NavMeshAgent navMeshAgent;
    VillageNavigation villageNavigation;
    [SerializeField] GameObject door;

    bool normalPathfinding = true;
    bool pathing = false;
    bool isHexed = false;
    [SerializeField] float waitingTimer = 0.0f;
    NavArea currentNavArea = null;

    void Start()
    {
        villageNavigation = VillageNavigation.instance;
    }
    
    void FixedUpdate()
    {
        pathing = navMeshAgent.hasPath && navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance;

        if (normalPathfinding)
        {
            // Debug.Log(navMeshAgent.hasPath);
            // during normal pathfinding
            if (pathing)
            {
                // assumes path endpoint is reachable
                // nothing i guess
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
            if (isHexed)
            {
                if (!pathing)
                {
                    // got to a house.
                    Destroy(this.gameObject);
                }
            }
        }
    }

    public void Hex()
    {
        // villager is hexed and will now kill themselves.
        isHexed = true;
        navMeshAgent.destination = door.transform.position;
    }
}