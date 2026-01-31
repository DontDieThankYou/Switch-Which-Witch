using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    float angle = 45.0f;
    float range = 2.0f;
    GameObject player;
    void Start()
    {
        
    }


    void Update()
    {
        bool withinDistance = Vector2.Distance(DimensionConverter.XYZtoXZ(player.transform.position),
                                               DimensionConverter.XYZtoXZ(this.transform.position)) < range;

        bool withinCone = Vector2.Dot(DimensionConverter.XYZtoXZ(this.transform.forward.normalized),
                                      DimensionConverter.XYZtoXZ(player.transform.forward.normalized)) < Mathf.Cos(angle/2);

        // vision detection
        if (withinDistance && withinCone)
        {
            Debug.Log("detected");
        }
    }

    void FixedUpdate()
    {
        
    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.deepPink;

    //     UnityEditor.Handles.DrawWireDisc(this.transform.position, Vector3.back, range);

    //     Gizmos.color = Color.cyan;
        
    // }
}
