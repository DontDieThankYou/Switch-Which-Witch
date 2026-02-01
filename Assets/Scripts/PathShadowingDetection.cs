using UnityEngine;

public class PathShadowingDetection : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("meow");
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerController.instance.isShadowed = false;
            Debug.Log("not shadowed");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerController.instance.isShadowed = true;
            Debug.Log("shadowed");
        }
    }
}
