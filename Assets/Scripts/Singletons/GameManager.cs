using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager INSTANCE;

    void Awake()
    {
        INSTANCE = this;
    }

    public void StartGame()
    {
        HUDController.INSTANCE.ToggleHUD(true);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
