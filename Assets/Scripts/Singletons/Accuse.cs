using UnityEngine;
using UnityEngine.InputSystem;

public class Accuse : MonoBehaviour
{
    [SerializeField] GameObject accuseVolume;

    private PlayerController playerController;

    void ToggleAccuseMode()
    {
        if (playerController.isAccusing)
        {
            playerController.isAccusing = false;
            accuseVolume.SetActive(false);
        } else
        {
            if (playerController.isCrafting)
            {
                playerController.CraftCanceled();
            }
            playerController.InteractCanceled();

            playerController.isAccusing = true;
            accuseVolume.SetActive(true);
        }
    }
}