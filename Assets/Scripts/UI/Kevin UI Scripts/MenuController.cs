using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private CanvasGroup background;
    [SerializeField] private CanvasGroup content;
    

    void Awake()
    {
        
    }

    void Start()
    {
        
    }

    public void StartGame()
    {
        StartCoroutine(StartProcess());
    }

    private IEnumerator StartProcess()
    {
        content.DOFade(0f, 1f);
        yield return new WaitForSecondsRealtime(0.8f);
        background.DOFade(1f, 1f);
        yield return new WaitForSecondsRealtime(1.3f);

        //screen is fully black right now, do your stuff here
        // -kevin
        
        background.DOFade(0f, 1f);
        yield return new WaitForSecondsRealtime(1f);

        gameObject.SetActive(false);

        // call some gamemamanger shit
        GameManager.INSTANCE.StartGame();
    }

    public void Quit()
    {
        StartCoroutine(QuitProcess());
    }
    private IEnumerator QuitProcess()
    {
        content.DOFade(0f, 0.8f);
        yield return new WaitForSecondsRealtime(0.4f);
        background.DOFade(1f, 1f);
        yield return new WaitForSecondsRealtime(1.2f);
        Application.Quit();
    }

}
