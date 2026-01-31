using UnityEngine;

public class Billboard : MonoBehaviour
{
    // Use LateUpdate to ensure all camera movements are finalized this frame
    void LateUpdate()
    {
        // Make the sprite's forward direction match the camera's forward direction
        transform.forward = Camera.main.transform.forward;
    }
}
