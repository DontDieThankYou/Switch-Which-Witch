using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyActions : MonoBehaviour
{
    // regular pathfinding
    // regions with weights if nothing overrides

    [SerializeField] NavMeshAgent navMeshAgent;
    VillageNavigation villageNavigation;
    VillageParanoia villageParanoia;
    [SerializeField] GameObject door;

    bool normalPathfinding = true;
    bool pathing = false;
    public bool isHexed = false;
    [SerializeField] float waitingTimer = 0.0f;
    NavArea currentNavArea = null;

    void Awake()
    {
        VillageParanoia.villagers.Add(this.gameObject);
    }

    void Start()
    {
        villageNavigation = VillageNavigation.instance;
        villageParanoia = VillageParanoia.instance;
        PickNewLocation();
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
                    PickNewLocation();
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

        PlayerController.instance.suspicion += PlayerController.instance.hexPoints.suspicion;
        VillageParanoia.instance.Paranoia += PlayerController.instance.hexPoints.paranoia;
    }

    void PickNewLocation()
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

    void LynchAt(Vector3 position)
    {
        StartCoroutine(Lynch(position));
    }

    IEnumerator Lynch(Vector3 position)
    {
        Vector3 pyreLocation = new Vector3(1, 0, -6);
        normalPathfinding = false;

        navMeshAgent.speed = 20.0f;

        // set no destination
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(1.5f); // pause!

        // path to position for duration
        navMeshAgent.destination = position;
        navMeshAgent.isStopped = false;
        yield return new WaitForSeconds(4.0f); // become mob

        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(1.0f); // pause!

        // then walk towards offset position in town center
        navMeshAgent.destination = this.transform.position - position + pyreLocation;
        navMeshAgent.isStopped = false;
        yield return new WaitForSeconds(8.0f); // walking mob

        // spawn pyre here

        // then go to circle position, relative velocity (static duration)
        navMeshAgent.isStopped = true;
        Vector3 targetPosition = VillagePyreDestination.GetNextPosition();
        Vector3 startPosition = this.transform.position;

        int maxFrames = 180;
        for (int i = 0; i < maxFrames; ++i)
        {
            this.transform.position = Vector3.Lerp(startPosition, targetPosition, i / maxFrames);
            yield return new WaitForFixedUpdate();
        }

        // light pyre here

        // burn
        yield return new WaitForSeconds(5.0f); // BURNNNNNNNNNNNNNNNNNN

        // then resume normal pathfinding
        navMeshAgent.speed = 10.0f;
        normalPathfinding = true;
        navMeshAgent.isStopped = false;
    }
}