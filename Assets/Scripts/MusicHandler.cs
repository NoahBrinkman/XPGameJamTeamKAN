using DG.Tweening;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    [SerializeField] private AudioSource overWorldMusic;
    [SerializeField] private AudioSource minigameMusic;

    
    
    public void FadeOverWorldOut()
    {
        if (overWorldMusic.isPlaying)
        {
            overWorldMusic.Pause();
        }
    }

    public void FadeInOverworldAgain()
    {
        if(overWorldMusic.isPlaying)
            overWorldMusic.UnPause();
        else
        {
            overWorldMusic.Play();
        }
    }

    public void StartMinigameMusic()
    {
        Debug.Log("Start Minigame Music");
        minigameMusic.Play();
    }

    public void StopMinigame()
    {
        Debug.Log("Stop Minigame Music");
        minigameMusic.Stop();
    }

    public void ResetMusic()
    {
        overWorldMusic.Stop();
        overWorldMusic.volume = 1f;
        minigameMusic.volume = 1f;
        minigameMusic.Stop();
    }
}
