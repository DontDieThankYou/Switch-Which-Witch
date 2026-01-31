using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] float angle;
    [SerializeField] float range;
    PlayerController player;

    public bool playerInVision = false;

    void Start()
    {
        player = PlayerController.instance;
    }


    void Update()
    {
    }

    void FixedUpdate()
    {

        bool withinDistance = Vector2.Distance(DimensionConverter.XYZtoXZ(player.transform.position),
                                               DimensionConverter.XYZtoXZ(this.transform.position)) < range;

        bool withinCone = Vector2.Dot(DimensionConverter.XYZtoXZ(this.transform.forward.normalized),
                                      DimensionConverter.XYZtoXZ(player.transform.position - this.transform.position)) > Mathf.Cos(Mathf.Deg2Rad * angle/2);

        playerInVision = false;
        // vision detection
        Debug.Log(string.Format("within distance {0}, within cone {1}", withinDistance, withinCone));
        if (withinDistance && withinCone)
        {
            playerInVision = true;
        }

        if (playerInVision)
        {
            // TODO: todo.
            // player.suspicion += 1;
            Debug.LogWarning("player suspicion increase to implement");
        }
    }
}
