using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class VillagerSpawner : MonoBehaviour
{
    [SerializeField] int maxVillagers = 20;
    [SerializeField] List<AnimatorController> villtypes;
    [SerializeField] GameObject villPrefab;
    float timer = 0.0f;

    void FixedUpdate()
    {
        // add arbitrary delay
        if (VillageParanoia.villagers.Count < maxVillagers && timer <= 0.0f)
        {
            Debug.Log(House.doors.Count);
            SpawnVillager();
            timer = Random.Range(0.2f, 1.0f);
        }
        timer -= Time.fixedDeltaTime;
    }

    public void SpawnVillager()
    {
        Vector3 position = House.doors[Random.Range(0, House.doors.Count)];
        Debug.Log(position);
        int villType = Random.Range(0, villtypes.Count);
        
        GameObject villager = Instantiate(villPrefab, position, Quaternion.identity, this.transform);
        villager.GetComponentInChildren<Animator>().runtimeAnimatorController = villtypes[villType];
        villager.GetComponent<EnemyActions>().villType = villType;
    }
}
