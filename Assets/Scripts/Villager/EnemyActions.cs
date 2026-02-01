using System.Collections.Generic;
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

    bool normalPathfinding = true;
    bool pathing = false;
    public bool isHexed = false;
    [SerializeField] float waitingTimer = 0.0f;
    NavArea currentNavArea = null;
    bool isMoving = true;
    bool isAccused = false;
    bool susCaught = false;
    List<EnemyActions> villagers;
    public bool isLyncher = false;
    public int lynchedType = 0;
    public int villType = 0;

    void Awake()
    {
        VillageParanoia.villagers.Add(this.gameObject);
        villagers = new();
    }

    void Start()
    {
        villageNavigation = VillageNavigation.instance;
        villageParanoia = VillageParanoia.instance;
        PickNewLocation();
    }
    
    void FixedUpdate()
    {
        if(!isMoving) return;
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
        int index = Random.Range(0, House.doors.Count);
        Debug.Log(index + " : " + House.doors.Count);
        navMeshAgent.destination = House.doors[index];

        PlayerController.instance.IncrementSus(PlayerController.instance.hexPoints.suspicion);
        VillageParanoia.instance.paranoia += PlayerController.instance.hexPoints.paranoia;
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

    public void LynchAt(Vector3 position)
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
        while(!VillageParanoia.susCaught)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2f); // become mob

        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(1.0f); // pause!

        // then walk towards offset position in town center
        navMeshAgent.destination = pyreLocation;
        navMeshAgent.isStopped = false;
        while(!VillageParanoia.susTied)
        {
            yield return null;
        }

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

        Pyre.instance.DismissPyre();

        // then resume normal pathfinding
        navMeshAgent.speed = 10.0f;
        normalPathfinding = true;
        navMeshAgent.isStopped = false;
    }
    public void BeAccused()
    {
        isMoving = false;
        navMeshAgent.isStopped = true;
        isAccused = true;
        if(villagers.Count > 0)
        {
            villagers[0].MakeLyncher(villType);
            VillageParanoia.susCaught = true;
            // Do fx here
            Destroy(this);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent != null
            && other.transform.parent.TryGetComponent<EnemyActions>(out EnemyActions o))
        {
            if(isAccused)
            {
                o.MakeLyncher(villType);
                CatchSus();
            }
            villagers.Add(o);
        }
        
        if(other.CompareTag("Pyre") && isLyncher)
        {
            //Do VFX

            Pyre.instance.TiePyre(lynchedType);
        }
    }
    
    public void OnTriggerExit(Collider other)
    {
        if(other.transform.parent != null
            && other.transform.parent.TryGetComponent<EnemyActions>(out EnemyActions o))
        {
            villagers.Remove(o);
        }
    }
    void OnDestroy()
    {
        VillageParanoia.villagers.Remove(this.gameObject);
    }
    void CatchSus()
    {
        VillageParanoia.susCaught = true;
        // Do fx here
        Destroy(this);
    }
    public void MakeLyncher(int villType)
    {
        isLyncher = true;
        lynchedType = villType;
    }
}