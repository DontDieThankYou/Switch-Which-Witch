using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class GrassGenerator : MonoBehaviour
{
    [SerializeField] int amount;
    [SerializeField] List<GameObject> grass;
    [SerializeField] Transform min;
    [SerializeField] Transform max;
    [SerializeField] Transform inmin;
    [SerializeField] Transform inmax;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        for (int i = 0; i < amount; ++i)
        {
            Vector3 position = GetRandomPosition();
            GameObject agrass = grass[Random.Range(0, grass.Count)];
            GameObject grassInstance = Instantiate(agrass, position, Quaternion.identity, this.transform);
            grassInstance.transform.localScale = Vector3.one * Random.Range(1, 2);
        }
    }

    Vector3 GetRandomPosition()
    {
        bool foundPosition = false;
        float x;
        float z;
        while (!foundPosition)
        {
            // garbage.
            x = Random.Range(min.position.x, max.position.x);
            z = Random.Range(min.position.z, max.position.z);

            if (x < inmin.position.x || x > inmax.position.x || z < inmin.position.z || z > inmax.position.z)
            {
                return new Vector3(x, 0, z);
            }
        }
        return Vector3.zero;
    }
}
