using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable] public struct NavArea
{
    public Vector3 position;
    public float radius;
    public int weight;
}

public class VillageNavigation : MonoBehaviour
{
    public static VillageNavigation instance;
    [SerializeField] List<NavArea> navAreas;
    private int sumOfAreaWeights;

    void Awake()
    {
        if(instance != null) Destroy(this);
        
        instance = this;

        foreach (NavArea navArea in navAreas)
        {
            sumOfAreaWeights += navArea.weight;
        }
    }
    
    public Vector3 PickRandomWeightedLocation()
    {
        // picks a NavArea
        int value = Random.Range(0, sumOfAreaWeights);
        NavArea? navArea = null;
        foreach (NavArea navAreaInstance in navAreas)
        {
            if (value < navAreaInstance.weight) {
                navArea = navAreaInstance;
                break;
            }
            value -= navAreaInstance.weight;
        }

        Assert.IsTrue(navArea.HasValue);
        return PickLocationFromNavArea(navArea.Value);
    }

    Vector3 PickLocationFromNavArea(NavArea navArea)
    {
        // chooses a location from within the radius
        // this strategy weighs spots near the center higher

        float distance = Random.Range(0, navArea.radius);
        Vector3 direction = Random.rotation.eulerAngles;
        direction.y = 0;
        return navArea.position + direction * distance;
    }
}