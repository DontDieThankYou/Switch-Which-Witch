using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    float angle = 45.0f;
    float range = 2.0f;
    GameObject player;

    public bool playerInVision = false;

    void Start()
    {
        
    }


    void Update()
    {
        bool withinDistance = Vector2.Distance(DimensionConverter.XYZtoXZ(player.transform.position),
                                               DimensionConverter.XYZtoXZ(this.transform.position)) < range;

        bool withinCone = Vector2.Dot(DimensionConverter.XYZtoXZ(this.transform.forward.normalized),
                                      DimensionConverter.XYZtoXZ(player.transform.forward.normalized)) < Mathf.Cos(angle/2);

        playerInVision = false;
        // vision detection
        if (withinDistance && withinCone)
        {
            playerInVision = true;
        }
    }

    void FixedUpdate()
    {
        if (playerInVision)
        {
            // TODO: todo.
            // player.suspicion += 1;
            Debug.LogWarning("player suspicion increase to implement");
        }
    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.deepPink;

    //     UnityEditor.Handles.DrawWireDisc(this.transform.position, Vector3.back, range);

    //     Gizmos.color = Color.cyan;
        
    // }
}
