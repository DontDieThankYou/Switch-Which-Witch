using DG.Tweening;
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
        menu.GetComponent<MenuController>().background.DOFade(0f,1f);
        menu.GetComponent<MenuController>().content.DOFade(1f,0.8f);
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
