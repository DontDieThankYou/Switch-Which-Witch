using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class VillagerSpawner : MonoBehaviour
{
    [SerializeField] int maxVillagers = 20;
    [SerializeField] List<AnimatorController> villtypes;
    [SerializeField] List<Sprite> villsprites;
    [SerializeField] GameObject villPrefab;
    float timer = 0.0f;

    void FixedUpdate()
    {
        // add arbitrary delay
        if (VillageParanoia.villagers.Count < maxVillagers && timer <= 0.0f)
        {
            SpawnVillager();
            timer = Random.Range(0.2f, 1.0f);
        }
        timer -= Time.fixedDeltaTime;
    }

    public void SpawnVillager()
    {
        Vector3 position = House.doors[Random.Range(0, House.doors.Count)];
        int villType = Random.Range(0, villtypes.Count);
        
        GameObject villager = Instantiate(villPrefab, position, Quaternion.identity, this.transform);
        // does this do anything??
        villager.GetComponentInChildren<SpriteRenderer>().sprite = villsprites[villType];
        villager.GetComponentInChildren<Animator>().runtimeAnimatorController = villtypes[villType];
        villager.GetComponent<EnemyActions>().villType = villType;
    }
}
