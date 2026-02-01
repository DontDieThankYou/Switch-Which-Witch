using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor.Animations;
using UnityEngine.EventSystems;

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
    public bool isAccused = false;
    bool susCaught = false;
    public float pyreDetectDist = 10f;
    List<EnemyActions> villagers;
    public bool isLyncher = false;
    public int lynchedType = 0;
    public int villType = 0;
    bool isTouchingPyre;
    [SerializeField] Animator animController;
    public static bool isCrowding;
    public Coroutine lynchCoroutine;
    public ParticleSystem hexps;

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

    void Update()
    {
        if (navMeshAgent.velocity.sqrMagnitude <= 0)
        {
            animController.SetBool("isMoving", false); 
            animController.SetInteger("direction", 0);
        } else
        {
            animController.SetBool("isMoving", true); 
            Vector2 moveDir = DimensionConverter.XYZtoXZ(navMeshAgent.velocity);
            
            if (moveDir.x < 0) // Left
            {
                animController.SetInteger("direction", 3);
            }
            else if (moveDir.x > 0) // Right
            {
                animController.SetInteger("direction", 4);
            }
            else if (moveDir.y < 0) // Down
            {
                animController.SetInteger("direction", 1);
            }
            else if (moveDir.y > 0) // Up
            {
                animController.SetInteger("direction", 2);
            }
        }
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

        if((transform.position - Pyre.instance.transform.position).magnitude < pyreDetectDist)
        {
            isTouchingPyre = true;
            if(isLyncher) Pyre.instance.TiePyre(lynchedType);
        }
        else
        {
            isTouchingPyre = false;
        }
    }

    public void Hex()
    {
        // villager is hexed and will now kill themselves.
        hexps?.Play();
        isHexed = true;
        normalPathfinding = false;
        int index = Random.Range(0, House.doors.Count);
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
        if(lynchCoroutine != null)
        {
            StopCoroutine(lynchCoroutine);
            lynchCoroutine = null;
        }
        lynchCoroutine = StartCoroutine(Lynch(position));
    }
    public void SkipLynch()
    {
        if(lynchCoroutine != null)
        {
            StopCoroutine(lynchCoroutine);
            lynchCoroutine = null;
        }
        isAccused = false;
        susCaught = false;
        isLyncher = false;
        Pyre.instance.DismissPyre();
        normalPathfinding = true;
        // then resume normal pathfinding
        navMeshAgent.speed = 10.0f;
        normalPathfinding = true;
        navMeshAgent.isStopped = false;
        AudioManager.instance.crowdingSource.Stop();
        PickNewLocation();
    }
    IEnumerator Lynch(Vector3 position)
    {
        isMoving = false;
        Vector3 pyreLocation = new Vector3(1, 0, -6);
        normalPathfinding = false;

        navMeshAgent.speed = 20.0f;

        // set no destination
        navMeshAgent.isStopped = true;

        // path to position for duration
        navMeshAgent.destination = position;
        navMeshAgent.isStopped = false;
        VillageParanoia.instance.resetTimerTime();
        while(!VillageParanoia.susCaught)
        {
            yield return null;
        }
        AudioManager.instance.PlayAudioSource(true, AudioManager.instance.crowdingSource);
        // then walk towards offset position in town center
        navMeshAgent.destination = pyreLocation;
        
        VillageParanoia.instance.resetTimerTime();
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
        Pyre.instance.LightPyre();
        // burn
        float timer = 0;
        float initVolume = AudioManager.instance.crowdingSource.volume;
        while(timer < 5f)
        {
            timer += Time.deltaTime;
            yield return null;
            AudioManager.instance.crowdingSource.volume = Mathf.Lerp(initVolume, 0, timer/5f);
        }// BURNNNNNNNNNNNNNNNNNN
        Pyre.instance.DismissPyre();
        normalPathfinding = true;
        // then resume normal pathfinding
        navMeshAgent.speed = 10.0f;
        normalPathfinding = true;
        navMeshAgent.isStopped = false;
        AudioManager.instance.crowdingSource.Stop();
        
        if(VillageParanoia.instance.isLynchingPlayer) GameManager.INSTANCE.BringUpMenu();
    }
    public void BeAccused()
    {
        isMoving = false;
        navMeshAgent.isStopped = true;
        isAccused = true;
        if(villagers.Count > 0)
        {
            villagers[0].MakeLyncher(villType);
            VillageParanoia.instance.SetSusCaught(true);
            // Do fx here
            Destroy(this.gameObject);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerEnter: " + other.name);
        if(other.transform.parent != null
            && other.transform.parent.TryGetComponent<EnemyActions>(out EnemyActions o))
        {
            if(isAccused)
            {
                o.MakeLyncher(villType);
                CatchSus();
            }
            villagers.Add(o);
                Debug.Log(other.name);
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
        VillageParanoia.instance.SetSusCaught(true);
        // Do fx here
        Destroy(this.gameObject);
    }
    public void MakeLyncher(int villType)
    {
        VillageParanoia.villagers.Remove(this.gameObject);
        isLyncher = true;
        if(isTouchingPyre) Pyre.instance.TiePyre(lynchedType);
        lynchedType = villType;
    }
}