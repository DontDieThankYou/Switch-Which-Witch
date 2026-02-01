using UnityEngine;

public class PathShadowingDetection : MonoBehaviour
{
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerController.instance.isShadowed = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerController.instance.isShadowed = true;
        }
    }
}
