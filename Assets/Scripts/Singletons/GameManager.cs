using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager INSTANCE;
    public GameObject menu;

    void Awake()
    {
        INSTANCE = this;
    }

    public void StartGame()
    {
        HUDController.INSTANCE.ToggleHUD(true);
        PlayerController.instance.Reset();
    }
    public void BringUpMenu()
    {
        menu.SetActive(true);
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
