using UnityEngine;

public class VillageParanoia : MonoBehaviour
{
    public static VillageParanoia instance;

    public float paranoia = 0;
    public readonly float accusationThreshold = 10;

    void Awake()
    {
        if(instance != null) Destroy(this);
        
        instance = this;
    }

    public bool AttemptAccuse()
    {
        if (paranoia > accusationThreshold)
        {
            Accuse();
            return true;
        }
        return false;
    }

    public void Accuse()
    {
        Debug.LogError("not finished lol");
    }
}