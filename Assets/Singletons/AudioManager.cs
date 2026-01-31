using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private int audioSfxMax;
    private List<AudioSource> audioSources;
    private List<AudioSource> musicTracks;
    private float sfxVol;
    private float musVol;
    [SerializeField] private float sfxDefault;
    [SerializeField] private float musDefault;

    void Awake()
    {
        if(instance != null) Destroy(this);
        
        instance = this;
        MenuManager.gameStart += GameStart;
        audioSources = new();
        musicTracks = new();
    }

    private void GameStart()
    {
        
    }

    public void ResetSFXVol()
    {
        SetSFXVol(sfxDefault);
    }
    public void ResetMusVol()
    {
        SetMusVol(musDefault);
    }
    public void SetSFXVol(float vol)
    {
        sfxVol = vol;
        foreach(AudioSource AuSo in audioSources) 
            if(AuSo.isPlaying) AuSo.volume = sfxVol;
            else audioSources.Remove(AuSo);
    }
    public void SetMusVol(float vol)
    {
        musVol = vol;
        
        foreach(AudioSource AuSo in musicTracks) 
            if(AuSo.isPlaying) AuSo.volume = musVol;
            else audioSources.Remove(AuSo);
    }
    public void PlayAudioSource(bool isSFX, AudioSource source, ulong delay = 0)
    {
        List<AudioSource> ls = isSFX ? audioSources : musicTracks;
        //Lazy removal of done audio sources
        foreach(AudioSource AuSo in ls) if(!AuSo.isPlaying) ls.Remove(AuSo);
        
        ls.Add(source);
        source.Play(delay);
    }
    void OnDestroy()
    {
        if(instance != this) return;

        MenuManager.gameStart -= GameStart;
        foreach(AudioSource AuSo in audioSources) AuSo.Stop();
        audioSources.Clear();

        foreach(AudioSource AuSo in musicTracks) AuSo.Stop();
        musicTracks.Clear();
    }
}