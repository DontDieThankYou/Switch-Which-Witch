using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Accuse
{
    [SerializeField] GameObject accuseVolume;
    public static Accuse instance;
    public static Accuse Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new();
            }
            return instance;
        }
    }

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