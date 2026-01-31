using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class MenuManager
{
    public static Action gameStart;
    public static void StartGame()
    {
        gameStart?.Invoke();
    }
    public static void Quit()
    {
        Application.Quit();
    }
    public static void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
    public static void UpdateSound(float scale)
    {
        AudioManager.instance.SetSFXVol(scale);
    }
    public static void UpdateMusic(float scale)
    {
        AudioManager.instance.SetMusVol(scale);
    }
}
