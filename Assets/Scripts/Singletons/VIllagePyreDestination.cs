using System;
using TMPro;
using UnityEngine;

public static class VillagePyreDestination
{
    private static int initVillPerCircle = 5;
    private static float villPerCircleMult = 1.5f;
    private static float initialRad = 5.5f;
    private static float incrementRad = 1.5f;
    private static int curCircleCount = 0;
    private static int curCircleNum = 0;
    private static float prevCircleAngle = 0;
    // [SerializeField] GameObject Villager;
    [SerializeField] static Vector3 PyrePosition = new Vector3(0,0,-3.2f);

    // void Start()
    // {
    //     for(int i = 0; i < 100; i++)
    //     {
    //         Instantiate(Villager, GetNextPosition(), Villager.transform.localRotation);
    //     }
    // }
    public static void Reset()
    {
        curCircleCount = 0;
        curCircleNum = 0;
        prevCircleAngle = 0;
    }
    public static Vector3 GetNextPosition()
    {
        int numOfVillInCircle = (int) (initVillPerCircle * Mathf.Pow(villPerCircleMult, curCircleNum));
        float dist = 280f / numOfVillInCircle;
        if(curCircleCount == 0)
        {
            prevCircleAngle = 40 + UnityEngine.Random.Range(0, dist);
        }
        else
        {
            prevCircleAngle += dist;
        }
        Vector3 dir = new Vector3(Mathf.Sin(prevCircleAngle * Mathf.Deg2Rad), 0, Mathf.Cos(prevCircleAngle * Mathf.Deg2Rad));
        dir = dir * (initialRad + incrementRad * curCircleNum);

        curCircleCount++;
        if(curCircleCount >= numOfVillInCircle)
        {
            curCircleNum++;
            curCircleCount = 0;
        }
        
        return dir + PyrePosition;
    }
}
